using QFramework;
using UnityEngine;

// 定义游戏架构
public class FlappyBirdArchitecture : Architecture<FlappyBirdArchitecture>
{
    protected override void Init()
    {
        // 注册Model
        RegisterModel<IGameModel>(new GameModel());
        
        // 注册System
        RegisterSystem<ITubeSystem>(new TubeSystem());
        RegisterSystem<IPlayerSystem>(new PlayerSystem());
        RegisterSystem<IScoreSystem>(new ScoreSystem());
        RegisterSystem<IAudioSystem>(new AudioSystem());
        RegisterSystem<IBackgroundSystem>(new BackgroundSystem());
        RegisterSystem<IUISystem>(new UISystem());
        
        // 注册Utility
        RegisterUtility<IResourcesUtility>(new ResourcesUtility());
        RegisterUtility<IObjectPoolUtility>(new ObjectPoolUtility());
    }
}
