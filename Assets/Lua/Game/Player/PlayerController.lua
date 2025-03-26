PlayerController = {}

-- 属性
local rigidBody2D = nil
local animator = nil
local gameManager = nil
local ballPrefab = nil
local firePosition = nil
local playerRendererTransform = nil
local audioController = nil
local rotateCoefficient = 1.0
local flyForce = 6.0

-- 玩家状态枚举
local PlayerState = {
    Idle = 0,
    UpDown = 1,
    Fly = 2
}

-- 初始化函数
function PlayerController.Start(go)
    local playerGO = go or CS.UnityEngine.GameObject.Find("Player")
    
    -- 获取组件引用
    rigidBody2D = playerGO.GetComponent(typeof(CS.UnityEngine.Rigidbody2D))
    animator = playerGO.GetComponent(typeof(CS.UnityEngine.Animator))
    playerRendererTransform = playerGO.transform.Find("Renderer")
    
    -- 获取其他对象引用
    gameManager = CS.UnityEngine.GameObject.Find("GameManager"):GetComponent(typeof(CS.GameManager))
    audioController = CS.UnityEngine.GameObject.Find("AudioManager"):GetComponent(typeof(CS.AudioController))
    
    -- 获取预制体和生成位置
    local playerController = playerGO:GetComponent(typeof(CS.PlayerController))
    if playerController then
        ballPrefab = playerController.ballPrefab
        firePosition = playerController.firePosition
        rotateCoefficient = playerController.rotateCoefficient
        flyForce = playerController.flyForce
    end
end

-- 更新函数
function PlayerController.Update()
    if not gameManager.isGameStart then
        return
    end
    
    if CS.UnityEngine.Input.GetMouseButtonDown(0) or CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.C) then
        PlayerController.Fly()
        PlayerController.CreateBall()
    end
    
    if playerRendererTransform then
        -- 使用 DOTween 进行旋转，但我们在 Lua 中使用简单的方式模拟
        local targetRotation = CS.UnityEngine.Quaternion.Euler(0, 0, rigidBody2D.velocity.y * rotateCoefficient)
        playerRendererTransform.rotation = CS.UnityEngine.Quaternion.Lerp(
            playerRendererTransform.rotation, 
            targetRotation, 
            0.3
        )
    end
end

-- 飞行函数
function PlayerController.Fly()
    audioController:PlaySfx(audioController.jump)
    rigidBody2D.velocity = CS.UnityEngine.Vector2(0, flyForce)
end

-- 创建小球
function PlayerController.CreateBall()
    -- 生成球的随机位置偏移
    local positionOffset = CS.UnityEngine.Vector2(
        CS.UnityEngine.Random.Range(-0.2, 0.2),
        CS.UnityEngine.Random.Range(-0.2, 0.2)
    )
    
    -- 生成球的随机旋转角度
    local rotationOffset = CS.UnityEngine.Random.Range(-30, 30)
    
    -- 计算最终生成位置和旋转
    local spawnPosition = firePosition.position + CS.UnityEngine.Vector3(positionOffset.x, positionOffset.y, 0)
    local spawnRotation = CS.UnityEngine.Quaternion.Euler(0, 0, rotationOffset)
    
    -- 实例化球
    local ball = CS.UnityEngine.Object.Instantiate(ballPrefab, spawnPosition, spawnRotation)
    ball:SetActive(true)
end

-- 改变状态
function PlayerController.ChangeState(isFly, isSimulated)
    isSimulated = isSimulated or false
    
    if isFly then
        animator:SetInteger("state", PlayerState.Fly)
    else
        animator:SetInteger("state", PlayerState.UpDown)
    end
    
    rigidBody2D.simulated = isSimulated
end

return PlayerController 