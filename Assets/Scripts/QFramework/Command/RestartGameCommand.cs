using QFramework;
using UnityEngine.SceneManagement;

public class RestartGameCommand : AbstractCommand
{
    protected override void OnExecute()
    {
        SceneManager.LoadScene("MainScene");
    }
}