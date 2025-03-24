using QFramework;

public class PlayerJumpCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetSystem<IPlayerSystem>().Jump();
        this.GetSystem<IAudioSystem>().PlaySound("jump");
    }
}