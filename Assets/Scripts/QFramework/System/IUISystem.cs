using QFramework;
using UnityEngine;

public interface IUISystem : ISystem
{
    void ShowUI(GameObject ui);
    void HideUI(GameObject ui);
    void ShowUI(GameObject ui, float duration);
    void HideUI(GameObject ui, float duration);
}