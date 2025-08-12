using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
interface IManager
{
    void Init();
    void Clear();
}
public class Managers : MonoBehaviour
{
    static Managers s_instance;

    static Managers Instance { get { Init(); return s_instance; } }

    #region Static Managers

    TableDataManager _table = new TableDataManager();
    UserDataManager _user = new UserDataManager();
    GameDataManager _game = new GameDataManager();
    UIManager _ui = new UIManager();
    SceneManagerEx _scene = new SceneManagerEx();
    ResourceManager _res = new ResourceManager();
    PoolManager _pool = new PoolManager();
    ObjectManager _obj = new ObjectManager();
    public static TableDataManager Table => Instance?._table;
    public static UserDataManager User => Instance?._user;
    public static GameDataManager Game => Instance?._game;
    public static UIManager UI => Instance?._ui;
    public static SceneManagerEx Scene => Instance?._scene;
    public static ResourceManager Resource => Instance?._res;
    public static PoolManager Pool => Instance?._pool;
    public static ObjectManager Object => Instance?._obj;

    private static List<IManager> _managers = new List<IManager>()
    {
        Table,
        User,
        Game,
        UI,
        Scene,
        Resource,
        Pool,
        Object,
    };
    #endregion
    
    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

        }
    }

    public static void InitManagers()
    {
        _managers.ForEach(m => m.Init());
    }

    public static void ClearManagers()
    {
        _managers.ForEach(m => m.Clear());
    }
    
    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return (s_instance as MonoBehaviour).StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        (s_instance as MonoBehaviour).StopCoroutine(coroutine);
    }
}