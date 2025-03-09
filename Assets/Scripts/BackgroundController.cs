using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private Vector3 _startPosition;
    public float speed = 0.01f;
    public float offset = -0.01f;
    public bool isMove = true;
    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        if (!isMove) return;
        // 计算基于时间的移动量
        float moveDistance = speed * Time.deltaTime;

        if (transform.position.x < -7.23 + offset)
        {
            transform.position = _startPosition;   
        }
        transform.Translate(-moveDistance, 0, 0);
    }

    
    
}
