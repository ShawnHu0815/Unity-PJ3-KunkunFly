using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubesController : MonoBehaviour
{   
    public GameObject greenTubePrefab;
    public GameManager gameManager;
    private readonly List<GameObject> _tubes = new List<GameObject>();
    private bool _isTubesMove = true;
    public void Start()
    {
        StartCoroutine(SpawnTubes());
    }
    
    /// <summary>
    /// 开始所有柱子移动
    /// </summary>
    public void StartMove()
    {
        _isTubesMove = true;
        foreach (var tube in _tubes)
        {
            tube.GetComponent<TubeController>().isMove = true;
        }
    }
    
    /// <summary>
    /// 停止所有柱子移动
    /// </summary>
    public void StopMove()
    {   
        _isTubesMove = false;
        foreach (var tube in _tubes)
        {
            tube.GetComponent<TubeController>().isMove = false;
        }
    }
    
    public void SpawnOneTube()
    {   
        GameObject tube = Instantiate(greenTubePrefab, new Vector3(3.65f , 0, 0), Quaternion.identity);
        tube.GetComponent<TubeController>().RandomHeight();
        tube.transform.SetParent(this.transform); // 将 tube 设为当前脚本所在对象的子物体
        tube.SetActive(true);
        _tubes.Add(tube);
    }
    

    private IEnumerator SpawnTubes()
    {   
        while (true)
        {
            yield return new WaitForSeconds(1.6f);
            if (!gameManager.isGameStart) continue;
            if (!_isTubesMove) continue;
            SpawnOneTube();
        }
    }

    public void deleteTube(GameObject tube)
    {
        // 从列表中移除柱子
        _tubes.Remove(tube);
    }  
    

}
