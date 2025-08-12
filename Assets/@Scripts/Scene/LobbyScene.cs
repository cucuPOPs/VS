using static Define;
public class LobbyScene : SceneBase
{
    public override SceneType SceneType => SceneType.Lobby;

    public override void OnEnter()
    {
        Managers.InitManagers();
        Managers.UI.ShowSceneUI<UI_LobbyScene>();
        
    }

    public override void OnExit()
    {
    }
}