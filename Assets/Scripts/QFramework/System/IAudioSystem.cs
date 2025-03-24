using QFramework;
using UnityEngine;

public interface IAudioSystem : ISystem
{
    void PlayMusic(AudioClip clip);
    void PlaySound(AudioClip clip);
    void PlaySound(string soundName);
}