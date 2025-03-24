using QFramework;
using UnityEngine;
using System.Threading.Tasks;

namespace QFramework
{
    // 加载状态接口
    public interface ILoadingModel : IModel
    {
        BindableProperty<bool> IsLoading { get; }
        BindableProperty<float> LoadingProgress { get; }
        BindableProperty<string> CurrentLoadingResource { get; }
    }

    // 加载状态模型实现
    public class LoadingModel : AbstractModel, ILoadingModel
    {
        public BindableProperty<bool> IsLoading { get; } = new BindableProperty<bool>(false);
        public BindableProperty<float> LoadingProgress { get; } = new BindableProperty<float>(0f);
        public BindableProperty<string> CurrentLoadingResource { get; } = new BindableProperty<string>(string.Empty);

        protected override void OnInit()
        {
            // 初始化时重置加载状态
            IsLoading.Value = false;
            LoadingProgress.Value = 0f;
            CurrentLoadingResource.Value = string.Empty;
        }

        // 设置加载进度
        public void SetProgress(float progress, string resourceName = null)
        {
            LoadingProgress.Value = Mathf.Clamp01(progress);
            
            if (!string.IsNullOrEmpty(resourceName))
            {
                CurrentLoadingResource.Value = resourceName;
            }
        }

        // 开始加载
        public void StartLoading(string resourceName = null)
        {
            IsLoading.Value = true;
            LoadingProgress.Value = 0f;
            
            if (!string.IsNullOrEmpty(resourceName))
            {
                CurrentLoadingResource.Value = resourceName;
            }
        }

        // 完成加载
        public void FinishLoading()
        {
            IsLoading.Value = false;
            LoadingProgress.Value = 1f;
            CurrentLoadingResource.Value = string.Empty;
        }
    }
}