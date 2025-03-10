using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rigidBody2D;
    public Animator animator;
    public GameManager gameManager;
    public GameObject ballPrefab;
    public Transform firePosition;
    public float rotateCoefficient = 1f;
    public float flyForce = 6f;
    public Transform playerRendererTransform;
    public AudioController audioController;
    
    private enum PlayerState
    {
        Idle,   // 0
        UpDown, // 1
        Fly     // 2
    }

    // Update is called once per frame
    void Update()
    {   
        if(!gameManager.isGameStart)return;
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.C))
        {
             Fly();
             CreateBall();
        }
        playerRendererTransform.DORotateQuaternion(Quaternion.Euler(0, 0, rigidBody2D.velocity.y * rotateCoefficient), 0.3f);
    }

    public void Fly()
    {   
        audioController.PlaySfx(audioController.jump);
        rigidBody2D.velocity = new Vector2(0, flyForce);
    }
    
    public void CreateBall()
    {
        // 生成球的随机位置偏移
        Vector2 positionOffset = new Vector2(
            Random.Range(-0.2f, 0.2f), // X 轴偏移（-0.1 到 0.1）
            Random.Range(-0.2f, 0.2f)  // Y 轴偏移（-0.1 到 0.1）
        );

        // 生成球的随机旋转角度
        float rotationOffset = Random.Range(-30f, 30f); // 旋转角度偏移

        // 计算最终生成位置和旋转
        Vector2 spawnPosition = firePosition.position + (Vector3)positionOffset;
        Quaternion spawnRotation = Quaternion.Euler(0, 0, rotationOffset);

        // 实例化球
        GameObject ball = Instantiate(ballPrefab, spawnPosition, spawnRotation);
        ball.SetActive(true);
    }

    public void ChangeState(bool isFly, bool isSimulated = false)
    {
        if (isFly)
        {   
            animator.SetInteger("state", (int)PlayerState.Fly);
        }
        else
        {
            animator.SetInteger("state", (int)PlayerState.UpDown);

        }
        rigidBody2D.simulated = isSimulated;
    }

}
