using QFramework;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameStartCommand : AbstractCommand
{
    protected override async void OnExecute()
    {
        // 预加载常用资源
        await PreloadCommonAssets();
        
        // 其他游戏启动逻辑
        var gameModel = this.GetModel<IGameModel>();
        gameModel.IsGameReady.Value = true;
    }
    
    private async Task PreloadCommonAssets()
    {
        var resourceUtil = this.GetUtility<IResourcesUtility>();
        
        // 并行加载多个资源
        var tasks = new List<Task>
        {
            resourceUtil.LoadAssetAsync<GameObject>("Prefabs/GreenTube"),
            resourceUtil.LoadAssetAsync<AudioClip>("Sounds/Jump"),
            resourceUtil.LoadAssetAsync<AudioClip>("Sounds/Score")
        };
        
        await Task.WhenAll(tasks);
    }
}