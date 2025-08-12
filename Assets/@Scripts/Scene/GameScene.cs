using UnityEngine;
using static Define;
public class GameScene : SceneBase
{
    public override SceneType SceneType => SceneType.Game;
    WaveManager _waveManager;
    public override void OnEnter()
    {
        Managers.Object.Spawn<UnitPlayer>(Vector3.zero);
        Managers.UI.ShowSceneUI<UI_Joystick>();
        _waveManager = gameObject.AddComponent<WaveManager>();

        //test
        Managers.Object.Spawn<MagnetItem>(new Vector3(-1, -3, 0));
        Managers.Object.Spawn<MagnetItem>(new Vector3(-3, -3, 0));
        
    }

    public override void OnExit()
    {
    }
    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(mapName);
        objMap.transform.position = Vector3.zero;
        objMap.name = "@Map";
        var map = objMap.GetComponent<InfiniteMap>();
        map.Init();
    }
}