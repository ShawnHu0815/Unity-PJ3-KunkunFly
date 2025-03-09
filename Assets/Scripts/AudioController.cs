using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    public AudioClip bgm;
    public AudioClip jump;
    public AudioClip getScore;
    public AudioClip highScore;
    public AudioClip death;

    private void Start()
    {
        bgmAudio.clip = bgm;
        bgmAudio.loop = true;
        bgmAudio.Play();
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxAudio.PlayOneShot(clip);
    }
}
