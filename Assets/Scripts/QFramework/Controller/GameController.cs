using QFramework;
using UnityEngine;
using UnityEngine.UI;

// 新的游戏控制器，与原GameManager并存
public class GameController : MonoBehaviour, IController
{
    public GameObject mainMenu;
    public GameObject tutorial;
    public GameObject scoreUI;
    public GameObject gameOver;
    public Text scoreText;
    public Text curScore;
    public Text bestScore;
    public GameObject newBestMark;
    public GameObject player;
    public Image medal;
    public List<Sprite> medalList;
    
    private IArchitecture mArchitecture;
    
    public IArchitecture GetArchitecture()
    {
        return mArchitecture ?? (mArchitecture = FlappyBirdArchitecture.Interface);
    }
    
    private IGameModel GameModel => this.GetModel<IGameModel>();
    
    private void Start()
    {
        // 注册游戏状态变化事件
        GameModel.IsGameStart.RegisterWithInitValue(OnGameStartChanged)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        
        GameModel.CurrentScore.RegisterWithInitValue(OnScoreChanged)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        
        // 注册游戏事件
        this.RegisterEvent<GameOverEvent>(OnGameOver)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
        
        this.RegisterEvent<AddScoreEvent>(OnAddScore)
            .UnRegisterWhenGameObjectDestroyed(gameObject);
    }
    
    // 开始按钮点击
    public void OnStartButtonClick()
    {
        this.SendCommand(new StartGameCommand());
    }
    
    private void OnGameStartChanged(bool isStart)
    {
        if (isStart)
        {
            // 使用 IUISystem 隐藏教程并显示得分UI
            this.GetSystem<IUISystem>().HideUI(tutorial);
            this.GetSystem<IUISystem>().ShowUI(scoreUI);
            
            // 玩家开始飞行
            this.GetSystem<IPlayerSystem>().Jump();
            this.GetSystem<IPlayerSystem>().CreateBall();
            this.GetSystem<IPlayerSystem>().SetPlayerState(true, true);
            
            // 启动管道生成
            this.GetSystem<ITubeSystem>().StartTubeSpawning();
            this.GetSystem<ITubeSystem>().StartTubeMovement();
            
            // 启动背景移动
            this.GetSystem<IBackgroundSystem>().StartMoving();
        }
    }
    
    private void OnGameOver(GameOverEvent e)
    {
        GameModel.IsGameReady.Value = false;
        GameModel.IsGameStart.Value = false;
        
        // 停止系统
        this.GetSystem<ITubeSystem>().StopTubeMovement();
        this.GetSystem<ITubeSystem>().StopTubeSpawning();
        
        // 停止背景移动
        this.GetSystem<IBackgroundSystem>().StopMoving();
        
        // 显示游戏结束UI
        this.GetSystem<IUISystem>().ShowUI(tutorial);
        this.GetSystem<IUISystem>().HideUI(scoreUI);

        // 更新UI
        UpdateGameOverUI();
    }
    
    private void OnAddScore(AddScoreEvent e)
    {
        this.GetSystem<IScoreSystem>().AddScore();
    }
    
    private void OnScoreChanged(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }
    
    private void UpdateGameOverUI()
    {
        int score = GameModel.CurrentScore.Value;
        
        if (curScore != null)
        {
            curScore.text = score.ToString();
        }
        
        if (bestScore != null)
        {
            bestScore.text = GameModel.BestScore.Value.ToString();
        }
        
        // 检查是否是新的最高分
        bool isNewBest = this.GetSystem<IScoreSystem>().CheckHighScore();
        if (isNewBest && newBestMark != null)
        {
            newBestMark.SetActive(true);
            this.GetSystem<IAudioSystem>().PlaySound("highscore");
        }
        else
        {
            if (newBestMark != null)
            {
                newBestMark.SetActive(false);
            }
            this.GetSystem<IAudioSystem>().PlaySound("death");
        }
        
        // 设置奖牌
        if (medal != null && medalList != null && medalList.Count >= 4)
        {
            switch (score)
            {
                default:
                    medal.gameObject.SetActive(false);
                    break;
        
                case int n when (n >= 10 && n < 20):
                    medal.sprite = medalList[0];
                    medal.gameObject.SetActive(true);
                    break;
        
                case int n when (n >= 20 && n < 50):
                    medal.sprite = medalList[1];
                    medal.gameObject.SetActive(true);
                    break;
        
                case int n when (n >= 50 && n < 100):
                    medal.sprite = medalList[2];
                    medal.gameObject.SetActive(true);
                    break;
        
                case int n when (n >= 100):
                    medal.sprite = medalList[3];
                    medal.gameObject.SetActive(true);
                    break;
            }
        }
    }
    
    // 重新开始游戏
    public void Restart()
    {
        this.SendCommand(new RestartGameCommand());
    }

    private void OnGameReadyChanged(bool isReady)
    {
        if (isReady)
        {
            // 新代码
            this.GetSystem<IUISystem>().HideUI(mainMenu);
            this.GetSystem<IUISystem>().ShowUI(tutorial);
            
            // 设置玩家状态
            this.GetSystem<IPlayerSystem>().SetPlayerState(true);
        }
    }
}