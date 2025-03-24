using QFramework;
using UnityEngine;

public class TubeController : MonoBehaviour, IController
{
    private IArchitecture mArchitecture;
    
    public IArchitecture GetArchitecture()
    {
        return mArchitecture ?? (mArchitecture = FlappyBirdArchitecture.Interface);
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 发送事件而不是直接调用GameManager
            this.SendEvent(new GameOverEvent());
        }
    }
}