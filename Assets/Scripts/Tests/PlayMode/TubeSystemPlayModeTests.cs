using NUnit.Framework;
using UnityEngine;
using QFramework;
using System.Collections;

public class TubeSystemPlayModeTests
{
    private TubeSystem _tubeSystem;
    private GameObject _testObject;
    private IGameModel _gameModel;

    [SetUp]
    public void Setup()
    {
        _testObject = new GameObject("TestObject");
        _tubeSystem = _testObject.AddComponent<TubeSystem>();
        _gameModel = _tubeSystem.GetModel<IGameModel>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_testObject);
    }

    [UnityTest]
    public IEnumerator TubeSystem_StartTubeSpawning_ShouldSpawnTubes()
    {
        // 设置游戏开始状态
        _gameModel.IsGameStart.Value = true;
        
        // 开始生成管道
        _tubeSystem.StartTubeSpawning();
        
        // 等待一段时间让管道生成
        yield return new WaitForSeconds(2f);
        
        // 获取活跃管道列表
        var activeTubesField = typeof(TubeSystem).GetField("_activeTubes", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var activeTubes = (System.Collections.Generic.List<GameObject>)activeTubesField.GetValue(_tubeSystem);
        
        // 验证是否有管道生成
        Assert.That(activeTubes.Count, Is.GreaterThan(0));
    }

    [UnityTest]
    public IEnumerator TubeSystem_StartTubeMovement_ShouldMoveTubes()
    {
        // 创建一个测试管道
        GameObject testTube = new GameObject("TestTube");
        var tubeController = testTube.AddComponent<TubeController>();
        
        // 添加到活跃列表
        var activeTubesField = typeof(TubeSystem).GetField("_activeTubes", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var activeTubes = (System.Collections.Generic.List<GameObject>)activeTubesField.GetValue(_tubeSystem);
        activeTubes.Add(testTube);
        
        // 开始移动
        _tubeSystem.StartTubeMovement();
        
        // 记录初始位置
        Vector3 initialPosition = testTube.transform.position;
        
        // 等待一段时间
        yield return new WaitForSeconds(0.1f);
        
        // 验证位置是否改变
        Assert.That(testTube.transform.position, Is.Not.EqualTo(initialPosition));
        
        Object.DestroyImmediate(testTube);
    }

    [UnityTest]
    public IEnumerator TubeSystem_StopTubeMovement_ShouldStopMoving()
    {
        // 创建一个测试管道
        GameObject testTube = new GameObject("TestTube");
        var tubeController = testTube.AddComponent<TubeController>();
        
        // 添加到活跃列表
        var activeTubesField = typeof(TubeSystem).GetField("_activeTubes", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var activeTubes = (System.Collections.Generic.List<GameObject>)activeTubesField.GetValue(_tubeSystem);
        activeTubes.Add(testTube);
        
        // 开始移动
        _tubeSystem.StartTubeMovement();
        
        // 等待一小段时间
        yield return new WaitForSeconds(0.1f);
        
        // 记录移动后的位置
        Vector3 positionBeforeStop = testTube.transform.position;
        
        // 停止移动
        _tubeSystem.StopTubeMovement();
        
        // 等待一小段时间
        yield return new WaitForSeconds(0.1f);
        
        // 验证位置是否保持不变
        Assert.That(testTube.transform.position, Is.EqualTo(positionBeforeStop));
        
        Object.DestroyImmediate(testTube);
    }
} 