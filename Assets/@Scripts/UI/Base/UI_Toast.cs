using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastUIData : BaseUIData
{
    public string msg;
}
public class UI_Toast : UI_Base
{
    #region Enum

    enum Images
    {
        BackgroundImage
    }

    enum Texts
    {
        ToastMessageValueText,
    }
    #endregion
    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        #region Object Bind
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        #endregion

        return true;
    }
    
    public override void SetInfo(BaseUIData uiData)
    {
        var data = uiData as ToastUIData;
        if (data == null)
        {
            Debug.LogError("UI_Toast SetInfo: Invalid data type.");
            return;
        }
        // 메시지 변경
        GetText((int)Texts.ToastMessageValueText).text = data.msg;
    }

    public override void ShowUI()
    {
        PopupOpenAnimation();
    }

    private void PopupOpenAnimation()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(Vector3.one * 1.2f, 0.2f).SetEase(Ease.OutBack));
        sequence.Append(transform.DOScale(Vector3.one, 0.1f).SetEase(Ease.Linear));
        sequence.AppendInterval(2f);
        sequence.Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InBack));
        sequence.OnComplete(() => CloseUI());
    }

    public override void CloseUI()
    {
        
    }
    
}
