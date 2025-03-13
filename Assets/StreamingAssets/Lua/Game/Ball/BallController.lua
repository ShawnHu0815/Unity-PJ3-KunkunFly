-- BallController.lua
-- 使用全局变量BallController代替局部变量，便于C#调用
BallController = {}

-- 初始化参数
rotationSpeed = 10
screenThreshold = -0.3
mainCamera = nil
transform = nil
gameObject = nil

-- 初始化函数，相当于 Start()
function BallController.Start(go)
    gameObject = go
    transform = go.transform
    mainCamera = CS.UnityEngine.Camera.main
end

-- 更新函数，相当于 FixedUpdate()
function BallController.FixedUpdate()
    -- 球自动旋转
    if transform ~= nil then
        transform:Rotate(0, 0, rotationSpeed)
    end
    
    -- 检查是否超出屏幕
    if BallController.IsOffScreen() then
        BallController.DestroyBall()
    end
end

-- 删除小球
function BallController.DestroyBall()
    CS.UnityEngine.Object.Destroy(gameObject)
end

-- 检查是否超出屏幕
function BallController.IsOffScreen()
    if mainCamera == nil or transform == nil then
        return false
    end
    
    local screenPoint = mainCamera:WorldToViewportPoint(transform.position)
    return screenPoint.x < screenThreshold
end

-- 返回模块
return BallController 