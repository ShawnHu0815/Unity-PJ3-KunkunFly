using QFramework;
using System.Collections.Generic;
using UnityEngine;

public interface ITubeSystem : ISystem
{
    void SpawnTube();
    void StartTubeMovement();
    void StopTubeMovement();
    void StartTubeSpawning();
    void StopTubeSpawning();
}