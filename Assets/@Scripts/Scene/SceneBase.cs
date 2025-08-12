using UnityEngine;
using static Define;
public abstract class SceneBase : MonoBehaviour
{
    public abstract SceneType SceneType { get; }
    public abstract void OnEnter();
    public abstract void OnExit();

}