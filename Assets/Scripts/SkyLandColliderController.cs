using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyLandColliderController : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {   
        // Debug.Log("检测到与天空和地面的碰撞");
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
}
