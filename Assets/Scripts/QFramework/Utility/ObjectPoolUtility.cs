using QFramework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolUtility : IObjectPoolUtility
{
    private Dictionary<string, Queue<GameObject>> _pools = new Dictionary<string, Queue<GameObject>>();
    
    public GameObject GetObject(GameObject prefab)
    {
        string key = prefab.name;
        
        if (!_pools.ContainsKey(key))
        {
            _pools[key] = new Queue<GameObject>();
        }
        
        GameObject obj;
        if (_pools[key].Count > 0)
        {
            obj = _pools[key].Dequeue();
            obj.SetActive(true);
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
        string key = obj.name;
        
        if (!_pools.ContainsKey(key))
        {
            _pools[key] = new Queue<GameObject>();
        }
        
        obj.SetActive(false);
        _pools[key].Enqueue(obj);
    }
}