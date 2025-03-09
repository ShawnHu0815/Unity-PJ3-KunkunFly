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
        if (Input.GetMouseButtonDown(0))
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
        GameObject ball = Instantiate(ballPrefab, firePosition.position, Quaternion.identity);
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
