using QFramework;
using UnityEngine;

namespace QFramework
{
    // 接口定义
    public interface IResourcesUtility : IUtility
    {
        T Load<T>(string path) where T : UnityEngine.Object;
    }
    
    // 实现类
    public class ResourcesUtility : IResourcesUtility
    {
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
    }
}