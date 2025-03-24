using QFramework;
using UnityEngine;

public interface IObjectPoolUtility : IUtility
{
    GameObject GetObject(GameObject prefab);
    void RecycleObject(GameObject obj);
}