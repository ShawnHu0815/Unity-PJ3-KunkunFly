using QFramework;
using UnityEngine;

public class ScoreTriggerController : MonoBehaviour, IController
{
    private IArchitecture mArchitecture;
    
    public IArchitecture GetArchitecture()
    {
        return mArchitecture ?? (mArchitecture = FlappyBirdArchitecture.Interface);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {   
            this.GetSystem<IAudioSystem>().PlaySound("score");
            // 使用事件而不是直接调用
            this.SendEvent(new AddScoreEvent());
        }
    }
}