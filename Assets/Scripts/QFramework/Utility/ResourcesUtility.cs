using QFramework;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QFramework
{
    // 扩展接口
    public interface IResourcesUtility : IUtility
    {
        // 同步加载(传统方式)
        T Load<T>(string path) where T : UnityEngine.Object;
        
        // 异步Addressable加载
        Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object;
        
        // 释放资源
        void Release<T>(T asset) where T : UnityEngine.Object;
    }
    
    // 实现类
    public class ResourcesUtility : IResourcesUtility
    {
        // 保存加载的资源引用
        private Dictionary<string, AsyncOperationHandle> _loadedAssets = 
            new Dictionary<string, AsyncOperationHandle>();
            
        // 传统加载方式
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
        
        // Addressables异步加载
        public async Task<T> LoadAssetAsync<T>(string address) where T : UnityEngine.Object
        {
            // 通知开始加载
            var loadingModel = this.GetModel<ILoadingModel>();
            if (loadingModel != null)
            {
                loadingModel.StartLoading(address);
            }
            
            try
            {
                // 如果已经加载过，直接返回
                if (_loadedAssets.TryGetValue(address, out AsyncOperationHandle handle))
                {
                    if (loadingModel != null) loadingModel.FinishLoading();
                    return (T)handle.Result;
                }
                
                // 开始异步加载
                var asyncHandle = Addressables.LoadAssetAsync<T>(address);
                _loadedAssets[address] = asyncHandle;
                
                // 等待加载完成，同时监控进度
                while (!asyncHandle.IsDone)
                {
                    if (loadingModel != null)
                    {
                        loadingModel.SetProgress(asyncHandle.PercentComplete, address);
                    }
                    await Task.Yield();
                }
                
                // 通知加载完成
                if (loadingModel != null) loadingModel.FinishLoading();
                
                // 返回加载的资源
                return asyncHandle.Result as T;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load asset {address}: {e.Message}");
                if (loadingModel != null) loadingModel.FinishLoading();
                return null;
            }
        }
        
        // 释放资源
        public void Release<T>(T asset) where T : UnityEngine.Object
        {
            foreach (var entry in _loadedAssets)
            {
                if (entry.Value.Result.Equals(asset))
                {
                    Addressables.Release(entry.Value);
                    _loadedAssets.Remove(entry.Key);
                    break;
                }
            }
        }
    }
}