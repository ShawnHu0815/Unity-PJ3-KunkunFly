TubesController = {}

-- 属性
local greenTubePrefab = nil
local gameManager = nil
local tubes = {}
local isTubesMove = true
local tubesGameObject = nil
local transform = nil
local spawnCoroutine = nil
local isInitialized = false

-- 初始化函数
function TubesController.Start(go)
    tubesGameObject = go
    transform = go.transform
    
    -- 获取GameManager和管道预制体引用
    gameManager = CS.UnityEngine.GameObject.Find("GameManager"):GetComponent(typeof(CS.GameManager))
    
    -- 从C#组件获取预制体
    local tubesController = go:GetComponent(typeof(CS.TubesController))
    if tubesController then
        greenTubePrefab = tubesController.greenTubePrefab
        isInitialized = true
    else
        CS.UnityEngine.Debug.LogError("无法获取 TubesController 组件")
        return
    end
    
    -- 启动生成管道的协程
    TubesController.StartSpawnCoroutine()
end

-- 启动生成管道的协程
function TubesController.StartSpawnCoroutine()
    if spawnCoroutine then
        CS.UnityEngine.MonoBehaviour.StopCoroutine(spawnCoroutine)
    end
    
    spawnCoroutine = CS.UnityEngine.MonoBehaviour.StartCoroutine(TubesController.SpawnTubes())
end

-- 生成管道的协程函数
function TubesController.SpawnTubes()
    return function()
        while true do
            CS.UnityEngine.WaitForSeconds(1.6)
            
            if not gameManager.isGameStart then
                goto continue
            end
            
            if not isTubesMove then
                goto continue
            end
            
            TubesController.SpawnOneTube()
            
            ::continue::
        end
    end
end

-- 生成一个管道
function TubesController.SpawnOneTube()
    if not isInitialized or not greenTubePrefab then
        return
    end
    
    local tube = CS.UnityEngine.Object.Instantiate(greenTubePrefab, CS.UnityEngine.Vector3(3.65, 0, 0), CS.UnityEngine.Quaternion.identity)
    
    -- 获取TubeController组件并设置随机高度
    local tubeController = tube:GetComponent(typeof(CS.TubeController))
    if tubeController then
        tubeController:RandomHeight()
    end
    
    -- 设置为当前对象的子物体
    tube.transform:SetParent(transform)
    tube:SetActive(true)
    
    -- 添加到管道列表中
    table.insert(tubes, tube)
end

-- 删除管道
function TubesController.DeleteTube(tube)
    -- 从列表中移除柱子
    for i, t in ipairs(tubes) do
        if t == tube then
            table.remove(tubes, i)
            break
        end
    end
end

-- 开始所有管道移动
function TubesController.StartMove()
    isTubesMove = true
    
    for _, tube in ipairs(tubes) do
        local tubeController = tube:GetComponent(typeof(CS.TubeController))
        if tubeController then
            tubeController.isMove = true
        end
    end
end

-- 停止所有管道移动
function TubesController.StopMove()
    isTubesMove = false
    
    for _, tube in ipairs(tubes) do
        local tubeController = tube:GetComponent(typeof(CS.TubeController))
        if tubeController then
            tubeController.isMove = false
        end
    end
end

return TubesController 