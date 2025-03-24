using System;
using System.Collections;
using UnityEngine;

public class MonoBehaviourRuntime : MonoBehaviour
{
    private static MonoBehaviourRuntime _instance;
    
    public static MonoBehaviourRuntime Instance
    {
        get
        {
            if (_instance == null)
            {
                var go = new GameObject("MonoBehaviourRuntime");
                _instance = go.AddComponent<MonoBehaviourRuntime>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    
    public void DelayCall(float delay, Action callback)
    {
        StartCoroutine(DelayCallCoroutine(delay, callback));
    }
    
    private IEnumerator DelayCallCoroutine(float delay, Action callback)
    {
        yield return new WaitForSeconds(delay);
        callback?.Invoke();
    }
}