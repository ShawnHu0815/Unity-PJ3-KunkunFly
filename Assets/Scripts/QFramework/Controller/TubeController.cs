using QFramework;
using UnityEngine;

public class TubeController : MonoBehaviour, IController
{
    private IArchitecture mArchitecture;
    public float speed = 0.05f;
    public bool isMove = true;
    
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
    
    public void RandomHeight()
    {
        float randomHeight = Random.Range(-1.86f, 1.86f);
        transform.position = new Vector3(transform.position.x, randomHeight, transform.position.z);
    }
    
    public void FixedUpdate()
    {
        // 如果不移动，直接返回
        if (!isMove)
        {
            return;
        }
        
        // 正常移动状态，向左移动
        transform.Translate(new Vector3(-speed, 0, 0));
    }
    
    // 重置状态方法，从对象池取出时调用
    public void ResetState()
    {
        isMove = true;
    }
}