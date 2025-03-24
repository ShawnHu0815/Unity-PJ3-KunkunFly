using QFramework;
using UnityEngine;
using DG.Tweening;

// 新的玩家控制器接口实现
public class NewPlayerController : MonoBehaviour, IController
{
    public Rigidbody2D rigidBody2D;
    public Animator animator;
    public GameObject ballPrefab;
    public Transform firePosition;
    public float rotateCoefficient = 1f;
    public float flyForce = 6f;
    public Transform playerRendererTransform;
    
    private IArchitecture mArchitecture;
    
    public IArchitecture GetArchitecture()
    {
        return mArchitecture ?? (mArchitecture = FlappyBirdArchitecture.Interface);
    }
    
    private IGameModel GameModel => this.GetModel<IGameModel>();
    
    private void Start()
    {
        // 设置玩家到系统中
        this.GetSystem<IPlayerSystem>().SetupPlayer(gameObject);
    }
    
    private void Update()
    {
        if (!GameModel.IsGameStart.Value) return;
        
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C))
        {
            // 使用命令而不是直接调用
            this.SendCommand(new PlayerJumpCommand());
            this.SendCommand(new CreateBallCommand());
        }
        
        // 处理旋转
        if (playerRendererTransform != null && rigidBody2D != null)
        {
            playerRendererTransform.DORotateQuaternion(
                Quaternion.Euler(0, 0, rigidBody2D.velocity.y * rotateCoefficient), 
                0.3f
            );
        }
    }

    public void ShowSpecialEffect(GameObject effectUI)
    {
        this.GetSystem<IUISystem>().ShowUI(effectUI);
    }
}