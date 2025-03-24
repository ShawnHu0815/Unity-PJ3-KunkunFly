using QFramework;
using UnityEngine;

namespace QFramework
{
    // 接口定义
    public interface IAudioSystem : ISystem
    {
        void PlayMusic(AudioClip clip);
        void PlaySound(AudioClip clip);
        void PlaySound(string soundName);
    }
    
    // 实现类
    public class AudioSystem : AbstractSystem, IAudioSystem
    {
        private AudioSource _bgmAudioSource;
        private AudioSource _sfxAudioSource;
        
        private AudioClip _jumpClip;
        private AudioClip _scoreClip;
        private AudioClip _deathClip;
        private AudioClip _highScoreClip;
        
        protected override void OnInit()
        {
            // 在初始化时获取音频源和音频剪辑
            var audioManager = GameObject.Find("AudioManager");
            if (audioManager != null)
            {
                var audioController = audioManager.GetComponent<AudioController>();
                _bgmAudioSource = audioController.GetComponents<AudioSource>()[0];
                _sfxAudioSource = audioController.GetComponents<AudioSource>()[1];
                
                _jumpClip = audioController.jump;
                _scoreClip = audioController.getScore;
                _deathClip = audioController.death;
                _highScoreClip = audioController.highScore;
            }
        }
        
        public void PlayMusic(AudioClip clip)
        {
            if (_bgmAudioSource != null)
            {
                _bgmAudioSource.clip = clip;
                _bgmAudioSource.loop = true;
                _bgmAudioSource.Play();
            }
        }
        
        public void PlaySound(AudioClip clip)
        {
            if (_sfxAudioSource != null)
            {
                _sfxAudioSource.PlayOneShot(clip);
            }
        }
        
        public void PlaySound(string soundName)
        {
            switch (soundName.ToLower())
            {
                case "jump":
                    PlaySound(_jumpClip);
                    break;
                case "score":
                    PlaySound(_scoreClip);
                    break;
                case "death":
                    PlaySound(_deathClip);
                    break;
                case "highscore":
                    PlaySound(_highScoreClip);
                    break;
            }
        }
    }
}