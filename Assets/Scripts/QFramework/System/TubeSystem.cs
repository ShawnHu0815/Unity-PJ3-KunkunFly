using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeSystem : AbstractSystem, ITubeSystem
{
    private List<GameObject> _tubes = new List<GameObject>();
    private GameObject _tubePrefab;
    private bool _isMoving = false;
    private bool _isSpawning = false;
    private float _spawnInterval = 1.6f;
    
    protected override void OnInit()
    {
        // 获取管道预制体
        _tubePrefab = Resources.Load<GameObject>("Prefabs/GreenTube");
        
        // 也可以通过查找场景中的对象获取
        var tubesController = GameObject.Find("Tubes");
        if (tubesController != null)
        {
            var controller = tubesController.GetComponent<TubesController>();
            if (controller != null)
            {
                _tubePrefab = controller.greenTubePrefab;
            }
        }
    }
    
    public void SpawnTube()
    {
        if (_tubePrefab != null)
        {
            GameObject tube = GameObject.Instantiate(_tubePrefab, new Vector3(3.65f, 0, 0), Quaternion.identity);
            var tubeController = tube.GetComponent<TubeController>();
            tubeController.RandomHeight();
            tubeController.isMove = _isMoving;
            
            // 找到Tubes对象并设置为父物体
            var tubesObj = GameObject.Find("Tubes");
            if (tubesObj != null)
            {
                tube.transform.SetParent(tubesObj.transform);
            }
            
            _tubes.Add(tube);
        }
    }
    
    public void StartTubeMovement()
    {
        _isMoving = true;
        foreach (var tube in _tubes)
        {
            if (tube != null)
            {
                tube.GetComponent<TubeController>().isMove = true;
            }
        }
    }
    
    public void StopTubeMovement()
    {
        _isMoving = false;
        foreach (var tube in _tubes)
        {
            if (tube != null)
            {
                tube.GetComponent<TubeController>().isMove = false;
            }
        }
    }
    
    public void StartTubeSpawning()
    {
        if (!_isSpawning)
        {
            _isSpawning = true;
            MonoBehaviourRuntime.Instance.StartCoroutine(SpawnTubesCoroutine());
        }
    }
    
    public void StopTubeSpawning()
    {
        _isSpawning = false;
    }
    
    private IEnumerator SpawnTubesCoroutine()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_spawnInterval);
            
            if (this.GetModel<IGameModel>().IsGameStart.Value && _isMoving)
            {
                SpawnTube();
            }
        }
    }
}