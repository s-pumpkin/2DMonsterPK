using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIEventListener : MonoBehaviour, IPointerClickHandler
{
    // 定義事件代理
    public delegate void UIEventProxy();
    //定義滑鼠點擊
    public event UIEventProxy OnClick;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
        {
            OnClick();
        }
    }
}
