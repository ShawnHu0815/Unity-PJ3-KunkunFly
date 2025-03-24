using UnityEngine;
using QFramework;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        // 确保MonoBehaviourRuntime存在
        var runtime = MonoBehaviourRuntime.Instance;
        
        // 初始化架构
        var architecture = FlappyBirdArchitecture.Interface;
        
        // 可以在这里进行一些全局初始化操作
        Debug.Log("QFramework架构已初始化");
    }
}