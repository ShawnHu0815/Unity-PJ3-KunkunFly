using QFramework;
using UnityEngine;

public interface IPlayerSystem : ISystem
{
    void SetupPlayer(GameObject player);
    void Jump();
    void CreateBall();
    void SetPlayerState(bool isFly, bool isSimulated = false);
}