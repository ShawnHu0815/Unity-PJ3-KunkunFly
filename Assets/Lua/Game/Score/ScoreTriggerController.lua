ScoreTriggerController = {}

-- 属性
local audioController = nil
local gameManager = nil

-- 初始化函数
function ScoreTriggerController.Start(go)
    -- 获取AudioController和GameManager引用
    audioController = CS.UnityEngine.GameObject.Find("AudioManager"):GetComponent(typeof(CS.AudioController))
    gameManager = CS.UnityEngine.GameObject.Find("GameManager"):GetComponent(typeof(CS.GameManager))
end

-- 碰撞触发处理
function ScoreTriggerController.OnTriggerEnter2D(collision)
    -- 播放得分音效
    if audioController then
        audioController:PlaySfx(audioController.getScore)
    end
    
    -- 更新得分
    if gameManager then
        gameManager:GetScore()
    end
end

return ScoreTriggerController 