using QFramework;
using UnityEngine;

public class ResourcesUtility : IResourcesUtility
{
    public T Load<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
}