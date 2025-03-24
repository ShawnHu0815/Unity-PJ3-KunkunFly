using QFramework;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

namespace QFramework
{
    // 接口定义
    public interface IUISystem : ISystem
    {
        void ShowUI(GameObject ui);
        void HideUI(GameObject ui);
        void ShowUI(GameObject ui, float duration);
        void HideUI(GameObject ui, float duration);
    }
    
    // 实现
    public class UISystem : AbstractSystem, IUISystem
    {
        // 缓存CanvasGroup组件
        private Dictionary<GameObject, CanvasGroup> _canvasGroups = new Dictionary<GameObject, CanvasGroup>();
        
        protected override void OnInit()
        {
            // 初始化代码
        }
        
        public void ShowUI(GameObject ui)
        {
            ShowUI(ui, 0.5f);
        }
        
        public void HideUI(GameObject ui)
        {
            HideUI(ui, 0.5f);
        }
        
        public void ShowUI(GameObject ui, float duration)
        {
            if (ui == null) return;
            
            ui.SetActive(true);
            
            CanvasGroup canvasGroup = GetCanvasGroup(ui);
            if (canvasGroup != null)
            {
                // 停止所有当前动画
                DOTween.Kill(canvasGroup);
                
                // 开始新的淡入动画
                canvasGroup.DOFade(1, duration);
            }
        }
        
        public void HideUI(GameObject ui, float duration)
        {
            if (ui == null) return;
            
            CanvasGroup canvasGroup = GetCanvasGroup(ui);
            if (canvasGroup != null)
            {
                // 停止所有当前动画
                DOTween.Kill(canvasGroup);
                
                // 开始新的淡出动画，完成后禁用GameObject
                canvasGroup.DOFade(0, duration).OnComplete(() => {
                    ui.SetActive(false);
                });
            }
            else
            {
                // 如果没有CanvasGroup，直接禁用
                ui.SetActive(false);
            }
        }
        
        private CanvasGroup GetCanvasGroup(GameObject ui)
        {
            // 检查缓存
            if (_canvasGroups.TryGetValue(ui, out var existingCanvasGroup))
            {
                return existingCanvasGroup;
            }
            
            // 查找或添加CanvasGroup组件
            CanvasGroup canvasGroup = ui.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = ui.AddComponent<CanvasGroup>();
            }
            
            // 添加到缓存
            _canvasGroups[ui] = canvasGroup;
            
            return canvasGroup;
        }
    }
}