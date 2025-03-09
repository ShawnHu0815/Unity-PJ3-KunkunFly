using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreTriggerController : MonoBehaviour
{
    private AudioController _audioController;

    private void Start()
    {
        _audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        _audioController.PlaySfx(_audioController.getScore);
        GameObject.Find("GameManager").GetComponent<GameManager>().GetScore();
    }
}
