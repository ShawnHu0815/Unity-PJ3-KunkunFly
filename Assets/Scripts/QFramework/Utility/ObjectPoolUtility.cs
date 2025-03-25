using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace QFramework
{
    // 接口定义
    public interface IObjectPoolUtility : IUtility
    {
        GameObject GetObject(GameObject prefab);
        void RecycleObject(GameObject obj);
        void PreWarm(GameObject prefab, int count);
        void Clear(string prefabName = null);
    }
    
    // 实现类
    public class ObjectPoolUtility : IObjectPoolUtility
    {
        private Dictionary<string, IObjectPool<GameObject>> _pools = new Dictionary<string, IObjectPool<GameObject>>();
        private Dictionary<string, GameObject> _prefabMap = new Dictionary<string, GameObject>();
        private Transform _poolRoot;
        
        public ObjectPoolUtility()
        {
            GameObject poolRootObj = new GameObject("ObjectPool");
            Object.DontDestroyOnLoad(poolRootObj);
            _poolRoot = poolRootObj.transform;
        }
        
        public GameObject GetObject(GameObject prefab)
        {
            string key = prefab.name;
            
            // 保存预制体引用
            if (!_prefabMap.ContainsKey(key))
            {
                _prefabMap[key] = prefab;
            }
            
            // 获取或创建对象池
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new ObjectPool<GameObject>(
                    createFunc: () => CreatePoolObject(prefab),
                    actionOnGet: (obj) => OnGetObject(obj),
                    actionOnRelease: (obj) => OnReleaseObject(obj),
                    actionOnDestroy: (obj) => OnDestroyObject(obj),
                    defaultCapacity: 10,
                    maxSize: 100
                );
            }
            
            return _pools[key].Get();
        }
        
        public void RecycleObject(GameObject obj)
        {
            if (obj == null) return;
            
            string key = obj.name;
            if (_pools.ContainsKey(key))
            {
                _pools[key].Release(obj);
            }
        }
        
        public void PreWarm(GameObject prefab, int count)
        {
            if (prefab == null || count <= 0) return;
            
            string key = prefab.name;
            
            // 保存预制体引用
            if (!_prefabMap.ContainsKey(key))
            {
                _prefabMap[key] = prefab;
            }
            
            // 确保对象池存在
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new ObjectPool<GameObject>(
                    createFunc: () => CreatePoolObject(prefab),
                    actionOnGet: (obj) => OnGetObject(obj),
                    actionOnRelease: (obj) => OnReleaseObject(obj),
                    actionOnDestroy: (obj) => OnDestroyObject(obj),
                    defaultCapacity: count,
                    maxSize: count * 2
                );
            }
            
            // 预热对象池
            var pool = _pools[key];
            var objects = new List<GameObject>();
            
            for (int i = 0; i < count; i++)
            {
                objects.Add(pool.Get());
            }
            
            foreach (var obj in objects)
            {
                pool.Release(obj);
            }
        }
        
        public void Clear(string prefabName = null)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                // 清空所有对象池
                foreach (var pool in _pools.Values)
                {
                    pool.Clear();
                }
                _pools.Clear();
                _prefabMap.Clear();
            }
            else if (_pools.ContainsKey(prefabName))
            {
                // 清空指定对象池
                _pools[prefabName].Clear();
                _pools.Remove(prefabName);
                _prefabMap.Remove(prefabName);
            }
        }
        
        // 创建池对象
        private GameObject CreatePoolObject(GameObject prefab)
        {
            var obj = GameObject.Instantiate(prefab);
            obj.name = prefab.name;
            obj.transform.SetParent(_poolRoot);
            obj.SetActive(false);
            return obj;
        }
        
        // 从池中获取对象时的处理
        private void OnGetObject(GameObject obj)
        {
            obj.SetActive(true);
            ResetObject(obj);
        }
        
        // 释放对象到池中时的处理
        private void OnReleaseObject(GameObject obj)
        {
            obj.transform.SetParent(_poolRoot);
            obj.SetActive(false);
        }
        
        // 销毁对象时的处理
        private void OnDestroyObject(GameObject obj)
        {
            if (obj != null)
            {
                GameObject.Destroy(obj);
            }
        }
        
        // 重置对象状态
        private void ResetObject(GameObject obj)
        {
            var resetComponents = obj.GetComponents<MonoBehaviour>();
            foreach (var component in resetComponents)
            {
                var resetMethod = component.GetType().GetMethod("ResetState");
                if (resetMethod != null)
                {
                    resetMethod.Invoke(component, null);
                }
            }
        }
    }
}