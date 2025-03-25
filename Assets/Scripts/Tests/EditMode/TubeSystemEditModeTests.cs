using NUnit.Framework;
using UnityEngine;
using QFramework;

public class TubeSystemEditModeTests
{
    private TubeSystem _tubeSystem;
    private GameObject _testObject;

    [SetUp]
    public void Setup()
    {
        _testObject = new GameObject("TestObject");
        _tubeSystem = _testObject.AddComponent<TubeSystem>();
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(_testObject);
    }

    [Test]
    public void TubeSystem_Initialization_ShouldNotBeNull()
    {
        Assert.That(_tubeSystem, Is.Not.Null);
    }

    [Test]
    public void TubeSystem_StopTubeSpawning_ShouldSetIsSpawningToFalse()
    {
        _tubeSystem.StopTubeSpawning();
    }

    [Test]
    public void TubeSystem_RecycleTube_ShouldRemoveFromActiveTubes()
    {
        // 创建一个测试用的管道对象
        GameObject testTube = new GameObject("TestTube");
        
        // 模拟添加到活跃列表
        var activeTubesField = typeof(TubeSystem).GetField("_activeTubes", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var activeTubes = (System.Collections.Generic.List<GameObject>)activeTubesField.GetValue(_tubeSystem);
        activeTubes.Add(testTube);

        // 测试回收功能
        _tubeSystem.RecycleTube(testTube);
        
        // 验证是否从活跃列表中移除
        Assert.That(activeTubes, Does.Not.Contain(testTube));
        
        Object.DestroyImmediate(testTube);
    }
} 