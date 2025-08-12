using System;
using static Define;
public class TitleScene : SceneBase
{
    public override SceneType SceneType => SceneType.Title;

    public override void OnEnter()
    {
        Managers.InitManagers();
    }

    public override void OnExit()
    {
    }

    private void Awake()
    {
        OnEnter();
    }
}