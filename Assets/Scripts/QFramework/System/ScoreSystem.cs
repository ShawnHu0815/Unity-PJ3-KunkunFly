using QFramework;

public class ScoreSystem : AbstractSystem, IScoreSystem
{
    private IGameModel _gameModel;
    
    protected override void OnInit()
    {
        _gameModel = this.GetModel<IGameModel>();
    }
    
    public void AddScore()
    {
        if (_gameModel.IsGameStart.Value)
        {
            _gameModel.CurrentScore.Value++;
            
            // 播放得分音效
            this.GetSystem<IAudioSystem>().PlaySound("score");
        }
    }
    
    public bool CheckHighScore()
    {
        int currentScore = _gameModel.CurrentScore.Value;
        int bestScore = _gameModel.BestScore.Value;
        
        if (currentScore > bestScore)
        {
            _gameModel.BestScore.Value = currentScore;
            return true;
        }
        
        return false;
    }
    
    public void ResetScore()
    {
        _gameModel.CurrentScore.Value = 0;
    }
}