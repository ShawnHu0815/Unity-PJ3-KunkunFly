using QFramework;

public interface IScoreSystem : ISystem
{
    void AddScore();
    bool CheckHighScore();
    void ResetScore();
}