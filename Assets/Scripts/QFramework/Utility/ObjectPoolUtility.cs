using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    // 接口定义
    public interface IObjectPoolUtility : IUtility
    {
        GameObject GetObject(GameObject prefab);
        void RecycleObject(GameObject obj);
        void PreWarm(GameObject prefab, int count); // 预热对象池
        void Clear(string prefabName = null); // 清空对象池
    }
    
    // 实现类
    public class ObjectPoolUtility : IObjectPoolUtility
    {
        private Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<string, GameObject> _prefabMap = new Dictionary<string, GameObject>(); // 存储预制体引用
        private Transform _poolRoot; // 对象池的根节点
        
        public ObjectPoolUtility()
        {
            // 创建对象池根节点
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
            
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new Queue<GameObject>();
            }
            
            GameObject obj;
            if (_pools[key].Count > 0)
            {
                obj = _pools[key].Dequeue();
                obj.SetActive(true);
                
                // 尝试调用重置方法
                ResetObject(obj);
            }
            else
            {
                obj = GameObject.Instantiate(prefab);
                obj.name = key;
            }
            
            return obj;
        }
        
        public void RecycleObject(GameObject obj)
        {
            if (obj == null) return;
            
            string key = obj.name;
            
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new Queue<GameObject>();
            }
            
            // 重置并禁用对象
            obj.transform.SetParent(_poolRoot);
            obj.SetActive(false);
            
            _pools[key].Enqueue(obj);
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
            
            if (!_pools.ContainsKey(key))
            {
                _pools[key] = new Queue<GameObject>();
            }
            
            // 预创建指定数量的对象
            for (int i = 0; i < count; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab);
                obj.name = key;
                obj.transform.SetParent(_poolRoot);
                obj.SetActive(false);
                _pools[key].Enqueue(obj);
            }
        }
        
        public void Clear(string prefabName = null)
        {
            if (string.IsNullOrEmpty(prefabName))
            {
                // 清空所有对象池
                foreach (var queue in _pools.Values)
                {
                    while (queue.Count > 0)
                    {
                        GameObject obj = queue.Dequeue();
                        if (obj != null)
                        {
                            GameObject.Destroy(obj);
                        }
                    }
                }
                _pools.Clear();
                _prefabMap.Clear();
            }
            else if (_pools.ContainsKey(prefabName))
            {
                // 清空指定对象池
                var queue = _pools[prefabName];
                while (queue.Count > 0)
                {
                    GameObject obj = queue.Dequeue();
                    if (obj != null)
                    {
                        GameObject.Destroy(obj);
                    }
                }
                _pools.Remove(prefabName);
                _prefabMap.Remove(prefabName);
            }
        }
        
        // 重置对象状态的私有方法
        private void ResetObject(GameObject obj)
        {
            // 尝试调用可能具有的重置方法
            var resetComponents = obj.GetComponents<MonoBehaviour>();
            foreach (var component in resetComponents)
            {
                // 使用反射查找并调用Reset或ResetState方法
                var resetMethod = component.GetType().GetMethod("ResetState");
                if (resetMethod != null)
                {
                    resetMethod.Invoke(component, null);
                }
            }
        }
    }
}