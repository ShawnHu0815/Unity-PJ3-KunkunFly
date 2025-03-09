using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject destoryPrefab;

    private void FixedUpdate()
    {
        // 球自动旋转
        transform.Rotate(0, 0, 30);
    }

    // 删除小球
    public void DestroyBall()
    {
        Destroy(gameObject);
        GameObject destoryParicle = Instantiate(destoryPrefab, transform.position, Quaternion.identity);
        Destroy(destoryParicle, 1);
    }
}
