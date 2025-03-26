GameLogic = {}

-- 属性
local gameManager = nil
local player = nil
local tubesController = nil
local backgroundController = nil
local audioController = nil
local isGameReady = false
local isGameStart = false
local canAcceptMouseInput = false

-- 初始化函数
function GameLogic.Start(go)
    -- 获取引用
    gameManager = CS.UnityEngine.GameObject.Find("GameManager"):GetComponent(typeof(CS.GameManager))
    player = CS.UnityEngine.GameObject.Find("Player")
    tubesController = CS.UnityEngine.GameObject.Find("Tubes"):GetComponent(typeof(CS.TubesController))
    backgroundController = CS.UnityEngine.GameObject.Find("Backgrounds"):GetComponent(typeof(CS.BackgroundController))
    audioController = CS.UnityEngine.GameObject.Find("AudioManager"):GetComponent(typeof(CS.AudioController))
end

-- 更新函数
function GameLogic.Update()
    -- 游戏准备但尚未开始
    if isGameReady and not isGameStart and canAcceptMouseInput then
        if CS.UnityEngine.Input.GetMouseButtonDown(0) then
            GameLogic.StartGame()
        end
    end
end

-- 开始按钮点击事件
function GameLogic.OnStartButtonClick()
    GameLogic.HandleButtonClick()
end

-- 处理按钮点击
function GameLogic.HandleButtonClick()
    -- 隐藏主菜单，显示教程
    local mainMenu = CS.UnityEngine.GameObject.Find("MainMenu")
    local tutorial = CS.UnityEngine.GameObject.Find("Tutorial")
    local playerController = player:GetComponent(typeof(CS.PlayerController))
    
    -- 使用工具函数隐藏/显示UI
    CS.Tools.Instance:HideUI(mainMenu)
    CS.Tools.Instance:ShowUI(tutorial)
    
    -- 改变玩家状态
    playerController:ChangeState(true)
    
    -- 设置游戏准备状态
    isGameReady = true
    
    -- 禁用开始按钮
    local startButton = CS.UnityEngine.GameObject.Find("StartButton"):GetComponent(typeof(CS.UnityEngine.UI.Button))
    startButton.interactable = false
    
    -- 一秒后允许鼠标输入
    CS.UnityEngine.MonoBehaviour.StartCoroutine(function()
        CS.UnityEngine.WaitForSecondsRealtime(1)
        canAcceptMouseInput = true
    end)
end

-- 开始游戏
function GameLogic.StartGame()
    -- 隐藏教程，显示分数UI
    local tutorial = CS.UnityEngine.GameObject.Find("Tutorial")
    local scoreUI = CS.UnityEngine.GameObject.Find("ScoreUI")
    
    CS.Tools.Instance:HideUI(tutorial)
    CS.Tools.Instance:ShowUI(scoreUI)
    
    -- 设置玩家动作
    local playerController = player:GetComponent(typeof(CS.PlayerController))
    playerController:Fly()
    playerController:CreateBall()
    playerController:ChangeState(true, true)
    
    -- 设置游戏开始状态
    isGameStart = true
end

-- 游戏结束
function GameLogic.GameOver()
    if not isGameStart then
        return
    end
    
    isGameReady = false
    isGameStart = false
    
    -- 停止移动
    tubesController:StopMove()
    
    local landsBackground = CS.UnityEngine.GameObject.Find("Lands"):GetComponent(typeof(CS.BackgroundController))
    backgroundController.isMove = false
    landsBackground.isMove = false
    
    -- 隐藏分数UI，显示游戏结束UI
    local scoreUI = CS.UnityEngine.GameObject.Find("ScoreUI")
    local gameOverUI = CS.UnityEngine.GameObject.Find("GameOver")
    
    CS.Tools.Instance:ShowUI(gameOverUI)
    CS.Tools.Instance:HideUI(scoreUI)
    
    -- 处理分数
    GameLogic.HandleGameOverScore()
end

-- 处理游戏结束时的分数逻辑
function GameLogic.HandleGameOverScore()
    local scoreText = CS.UnityEngine.GameObject.Find("ScoreText"):GetComponent(typeof(CS.UnityEngine.UI.Text))
    local curScore = CS.UnityEngine.GameObject.Find("CurScore"):GetComponent(typeof(CS.UnityEngine.UI.Text))
    local bestScore = CS.UnityEngine.GameObject.Find("BestScore"):GetComponent(typeof(CS.UnityEngine.UI.Text))
    local newBestMark = CS.UnityEngine.GameObject.Find("NewBestMark")
    local medal = CS.UnityEngine.GameObject.Find("Medal"):GetComponent(typeof(CS.UnityEngine.UI.Image))
    
    local score = tonumber(scoreText.text)
    local bestScoreValue = CS.UnityEngine.PlayerPrefs.GetInt("bestScore", 0)
    
    -- 如果当前分数大于最高分，则更新最高分，并播放高分音效
    if bestScoreValue < score then
        audioController:PlaySfx(audioController.highScore)
        CS.UnityEngine.PlayerPrefs.SetInt("bestScore", score)
        newBestMark:SetActive(true)
    else
        -- 否则播放死亡音效
        audioController:PlaySfx(audioController.death)
        newBestMark:SetActive(false)
    end
    
    curScore.text = tostring(score)
    bestScore.text = tostring(CS.UnityEngine.PlayerPrefs.GetInt("bestScore"))
    
    -- 根据分数设置奖牌
    GameLogic.SetMedalByScore(score, medal)
end

-- 根据分数设置奖牌
function GameLogic.SetMedalByScore(score, medal)
    -- 获取奖牌列表
    local medalList = gameManager.medalList
    
    -- 根据分数设置不同的奖牌
    if score < 10 then
        medal.gameObject:SetActive(false)
    elseif score >= 10 and score < 20 then
        medal.sprite = medalList[0]
        medal.gameObject:SetActive(true)
    elseif score >= 20 and score < 50 then
        medal.sprite = medalList[1]
        medal.gameObject:SetActive(true)
    elseif score >= 50 and score < 100 then
        medal.sprite = medalList[2]
        medal.gameObject:SetActive(true)
    elseif score >= 100 then
        medal.sprite = medalList[3]
        medal.gameObject:SetActive(true)
    end
end

-- 得分
function GameLogic.GetScore()
    if not isGameStart then
        return
    end
    
    local scoreText = CS.UnityEngine.GameObject.Find("ScoreText"):GetComponent(typeof(CS.UnityEngine.UI.Text))
    local currentScore = tonumber(scoreText.text) or 0
    scoreText.text = tostring(currentScore + 1)
end

-- 重新开始游戏
function GameLogic.Restart()
    CS.UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene")
end

return GameLogic 