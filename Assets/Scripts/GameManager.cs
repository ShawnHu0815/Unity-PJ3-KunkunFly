using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class GameManager : MonoBehaviour
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
    public AudioController audioController;
    public bool isGameReady = false;
    public bool isGameStart = false;
    private bool _canAcceptMouseInput = false;
    
    
    /// <summary>
    /// 处理开始按钮点击事件
    /// </summary>
    public void OnStartButtonClick()
    {   
        StartCoroutine(HandleButtonClick());
    }
    
    private IEnumerator HandleButtonClick()
    {
        Tools.Instance.HideUI(mainMenu);
        Tools.Instance.ShowUI(tutorial);
        player.GetComponent<PlayerController>().ChangeState(true);
        isGameReady = true;
        GameObject.Find("StartButton").GetComponent<Button>().interactable = false;

        // 等待1秒
        yield return new WaitForSecondsRealtime(1f);
        
        // 允许鼠标输入
        _canAcceptMouseInput = true;
    }
    
    /// <summary>
    /// 在游戏开始前，只处理一次鼠标点击事件
    /// </summary>
    private void Update()
    {   
        if (!isGameReady)
        {
            return;
        }
        if (isGameStart)
        {
            return;
        }
        // 只有在允许鼠标输入时才处理鼠标点击事件
        if (_canAcceptMouseInput && Input.GetMouseButtonDown(0))
        {  
            Tools.Instance.HideUI(tutorial);
            Tools.Instance.ShowUI(scoreUI);
            player.GetComponent<PlayerController>().Fly();
            player.GetComponent<PlayerController>().CreateBall();
            player.GetComponent<PlayerController>().ChangeState(true, true);
            isGameStart = true;
        }
    }
    
    /// <summary>
    /// 游戏结束逻辑
    /// </summary>
    public void GameOver()
    {
        if (!isGameStart) return;
        
        isGameReady = false;
        isGameStart = false;
        
        GameObject.Find("Tubes").GetComponent<TubesController>().StopMove();
        GameObject.Find("Backgrounds").GetComponent<BackgroundController>().isMove = false;
        GameObject.Find("Lands").GetComponent<BackgroundController>().isMove = false;
        
        Tools.Instance.ShowUI(gameOver);
        Tools.Instance.HideUI(scoreUI);
        
        int score = int.Parse(scoreText.text);
        
        // 如果当前分数大于最高分，则更新最高分，并播放高分音效
        if (PlayerPrefs.GetInt("bestScore") < score)
        {   
            audioController.PlaySfx(audioController.highScore);
            PlayerPrefs.SetInt("bestScore", score);
            newBestMark.SetActive(true);
        }
        // 否则播放死亡音效
        else
        {
            audioController.PlaySfx(audioController.death);
        }
        
        curScore.text = score.ToString();
        bestScore.text = PlayerPrefs.GetInt("bestScore").ToString();
        
        // 根据分数设置奖牌
        switch (score)
        {
            default:
                medal.gameObject.SetActive(false);
                break;
    
            case int n when (n >= 10 && n < 20):
                medal.sprite = medalList[0];
                break;
    
            case int n when (n >= 20 && n < 50):
                medal.sprite = medalList[1];
                break;
    
            case int n when (n >= 50 && n < 100):
                medal.sprite = medalList[2];
                break;
    
            case int n when (n >= 100):
                medal.sprite = medalList[3];
                break;
        }
        
    }
    
    /// <summary>
    /// 得分
    /// </summary>
    public void GetScore()
    {   
        if(!isGameStart) return;
        scoreText.text = (int.Parse(scoreText.text) + 1).ToString();
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
}
