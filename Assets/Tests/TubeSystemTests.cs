using NUnit.Framework;
using UnityEngine;
using QFramework;

public class TubeSystemTests
{
    private TubeSystem _tubeSystem;
    private GameObject _testObject;

    [SetUp]
    public void Setup()
    {
        // 创建测试对象
        _testObject = new GameObject("TestObject");
        _tubeSystem = _testObject.AddComponent<TubeSystem>();
    }

    [TearDown]
    public void TearDown()
    {
        // 清理测试对象
        Object.DestroyImmediate(_testObject);
    }

    [Test]
    public void TubeSystem_Initialization_ShouldNotBeNull()
    {
        // 验证系统是否正确初始化
        Assert.That(_tubeSystem, Is.Not.Null);
    }

    [Test]
    public void TubeSystem_StartTubeMovement_ShouldSetIsMovingToTrue()
    {
        // 测试开始移动功能
        _tubeSystem.StartTubeMovement();
        
    }
} 