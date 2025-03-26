TubeController = {}

-- 属性
local speed = 0.05
local isMove = true
local tubesController = nil
local mainCamera = nil
local tubeGameObject = nil
local transform = nil

-- 初始化函数
function TubeController.Start(go)
    tubeGameObject = go
    transform = go.transform
    
    -- 获取主摄像机和TubesController引用
    mainCamera = CS.UnityEngine.Camera.main
    tubesController = CS.UnityEngine.GameObject.Find("Tubes"):GetComponent(typeof(CS.TubesController))
end

-- 更新函数
function TubeController.FixedUpdate()
    -- 如果不移动，直接返回
    if not isMove then
        return
    end
    
    -- 正常移动状态，向左移动
    if transform then
        transform.position = transform.position + CS.UnityEngine.Vector3(-speed, 0, 0)
    end
    
    -- 检查柱子是否完全移出屏幕
    if TubeController.IsOffScreen() then
        TubeController.DeleteTube()
    end
end

-- 随机高度
function TubeController.RandomHeight()
    if transform then
        local randomHeight = CS.UnityEngine.Random.Range(-1.86, 1.86)
        transform.position = CS.UnityEngine.Vector3(transform.position.x, randomHeight, transform.position.z)
    end
end

-- 碰撞处理
function TubeController.OnTriggerEnter2D(collision)
    -- 如果碰撞到的是小鸟
    if collision.gameObject.tag == "Player" then
        -- 游戏结束
        local gameManager = CS.UnityEngine.GameObject.Find("GameManager"):GetComponent(typeof(CS.GameManager))
        gameManager:GameOver()
    end
end

-- 检查是否超出屏幕
function TubeController.IsOffScreen()
    if not mainCamera or not transform then
        return false
    end
    
    local screenPoint = mainCamera:WorldToViewportPoint(transform.position)
    return screenPoint.x < -0.3
end

-- 删除管道
function TubeController.DeleteTube()
    if tubesController and tubeGameObject then
        tubesController:deleteTube(tubeGameObject)
        CS.UnityEngine.Object.Destroy(tubeGameObject)
    end
end

-- 设置移动状态
function TubeController.SetMoveState(state)
    isMove = state
end

-- 设置速度
function TubeController.SetSpeed(newSpeed)
    speed = newSpeed
end

return TubeController 