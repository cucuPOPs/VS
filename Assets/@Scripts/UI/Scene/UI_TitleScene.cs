using System;
using DG.Tweening;

public class UI_TitleScene : UI_Scene
{
    #region Enum

    enum Buttons
    {
        StartButton
    }

    enum Texts
    {
        StartText
    }

    #endregion

    private void Awake()
    {
        Init();
        ShowUI();
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        // 오브젝트 바인딩
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.StartButton).gameObject.SetActive(false);
        GetButton((int)Buttons.StartButton).gameObject.BindEvent(() =>
        {
            //임시.바로게임씬으로.
            Managers.Scene.LoadScene(Define.SceneType.Game);
        });
        return true;
    }

    public override void ShowUI()
    {
        GetText((int)Texts.StartText).DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic).Play();
        GetButton((int)Buttons.StartButton).gameObject.SetActive(true);
    }

}