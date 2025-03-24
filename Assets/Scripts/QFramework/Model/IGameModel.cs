using QFramework;

public interface IGameModel : IModel
{
    BindableProperty<bool> IsGameReady { get; }
    BindableProperty<bool> IsGameStart { get; }
    BindableProperty<int> CurrentScore { get; }
    BindableProperty<int> BestScore { get; }
}