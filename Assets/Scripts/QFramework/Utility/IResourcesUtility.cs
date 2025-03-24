using QFramework;
using UnityEngine;

public interface IResourcesUtility : IUtility
{
    T Load<T>(string path) where T : UnityEngine.Object;
}