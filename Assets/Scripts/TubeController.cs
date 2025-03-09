using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TubeController : MonoBehaviour
{   
    public float speed = 0.05f;
    public bool isMove = true;
    public TubesController tubesController;
    private Camera _mainCamera;
    
    private void Start()
    {
        tubesController = GameObject.Find("Tubes").GetComponent<TubesController>();// 初始化tubesController
        _mainCamera = Camera.main; // 在Start方法中获取主摄像机
        // PlayerPrefs.SetInt("bestScore", 0);
    }
    
    public void FixedUpdate()
    {   
        // 如果不移动，直接返回
        if (!isMove)
        {
            return;
        }
        // 正常移动状态，向左移动
        transform.Translate(new Vector3(-speed, 0, 0));
        
        // 检查柱子是否完全移出屏幕
        if (IsOffScreen())
        {   
            tubesController.deleteTube(gameObject); // 在tubeController中调用deleteTube方法，删除柱子
            Destroy(gameObject); // 如果完全移出屏幕，则销毁对象
        }
    }

    public void RandomHeight()
    {
        float randomHeight = UnityEngine.Random.Range(-1.86f, 1.86f);
        transform.position = new Vector3(transform.position.x, randomHeight, transform.position.z);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {   
        // Debug.Log("检测到与柱子的碰撞");
        // 如果碰撞到的是小鸟
        if (other.gameObject.CompareTag("Player"))
        {
            // 游戏结束
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
        // 如果碰撞到的是球
        else
        {
           // 调用BallController中的Destory方法
              other.gameObject.GetComponent<BallController>().DestroyBall();
        }
    }
    
    private bool IsOffScreen()
    {
        Vector3 screenPoint = _mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3f;
    }
}
