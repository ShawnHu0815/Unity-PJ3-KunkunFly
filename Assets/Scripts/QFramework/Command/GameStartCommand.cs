using QFramework;

public class GameStartCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<IGameModel>().IsGameStart.Value = true;
    }
}