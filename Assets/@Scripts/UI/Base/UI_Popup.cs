using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIData : BaseUIData
{
    
}
public class UI_Popup : UI_Base
{
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, true);
        return true;
    }
    public override void SetInfo(BaseUIData data)
    {
        base.SetInfo(data);

        if (data is not PopupUIData _data)
        {
            Debug.LogError("data is invalid.");
            return;
        }
            
    }
    public override void ShowUI()
    {
    }
    public override void CloseUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
