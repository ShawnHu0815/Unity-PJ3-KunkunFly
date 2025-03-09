using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    #region 单例模式
    /// <summary>
    /// Tools单例模式
    /// </summary>
    private static Tools _instance;
    public static Tools Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Tools();
            }
            return _instance;
        }
    }
    Tools()
    {
        // 封锁构造函数
    }

    #endregion

    
    public void ShowUI(GameObject UIObject)
    {
        UIObject.SetActive(true);
        UIObject.GetComponent<CanvasGroup>().alpha = 0;
        UIObject.GetComponent<UIManager>().ShowUI();
    }
    
    public void HideUI(GameObject UIObject)
    {
        UIObject.GetComponent<UIManager>().HideUI();
    }
}
