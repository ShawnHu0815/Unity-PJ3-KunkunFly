using QFramework;

public class CreateBallCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IPlayerSystem>().CreateBall();
    }
}
