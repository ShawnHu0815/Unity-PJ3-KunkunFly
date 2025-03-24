using QFramework;
using UnityEngine;

public class StartGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        var gameModel = this.GetModel<IGameModel>();
        gameModel.IsGameReady.Value = true;
        
        // 使用MonoBehaviourRuntime延迟调用
        MonoBehaviourRuntime.Instance.DelayCall(1f, () => {
            gameModel.IsGameStart.Value = true;
        });
    }
}