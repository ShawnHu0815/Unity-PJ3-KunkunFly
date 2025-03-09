using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject destoryPrefab;
    private Camera _mainCamera;
    
    private void Start()
    {
        _mainCamera = Camera.main; // 在Start方法中获取主摄像机
    }
    
    private void FixedUpdate()
    {
        // 球自动旋转
        transform.Rotate(0, 0, 10);
        
        if (IsOffScreen())
        {   
            DestroyBall(); // 如果完全移出屏幕，则销毁对象
        }
    }

    // 删除小球
    public void DestroyBall()
    {
        Destroy(gameObject);
        // GameObject destoryParicle = Instantiate(destoryPrefab, transform.position, Quaternion.identity);
        // Destroy(destoryParicle, 1);
    }
    
    private bool IsOffScreen()
    {
        Vector3 screenPoint = _mainCamera.WorldToViewportPoint(transform.position);
        return screenPoint.x < -0.3f;
    }
}
