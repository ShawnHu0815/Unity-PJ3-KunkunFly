using QFramework;
using UnityEngine;

public class GameModel : AbstractModel, IGameModel
{
    public BindableProperty<bool> IsGameReady { get; } = new BindableProperty<bool>(false);
    public BindableProperty<bool> IsGameStart { get; } = new BindableProperty<bool>(false);
    public BindableProperty<int> CurrentScore { get; } = new BindableProperty<int>(0);
    public BindableProperty<int> BestScore { get; } = new BindableProperty<int>(0);

    protected override void OnInit()
    {
        // 从PlayerPrefs加载最高分
        BestScore.Value = PlayerPrefs.GetInt("bestScore", 0);
    }
}