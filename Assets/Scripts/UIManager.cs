using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public void ShowUI()
    {
        canvasGroup.DOFade(1, 0.5f);
    }
    
    public void HideUI()
    {
        canvasGroup.DOFade(0, 0.5f).onComplete = () =>
        {
            gameObject.SetActive(false);
        };
    }
}
