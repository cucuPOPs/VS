using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
public class SceneManagerEx : IManager
{
    public SceneBase curScene
    {
        get
        {
            if (_curScene == null)
            {
                _curScene = GameObject.FindAnyObjectByType<SceneBase>();
            }
            return _curScene;
        }
    }

    private SceneBase _curScene;
    private SceneTransition sceneTransition;

    public void LoadScene(SceneType sceneType)
    {
        if (sceneTransition == null)
        {
            sceneTransition = Managers.Resource.Instantiate("transition").GetComponent<SceneTransition>();
        }
        Managers.StartCoroutine(LoadSceneCoroutine(sceneType));
    }
    
    private IEnumerator LoadSceneCoroutine(SceneType sceneType)
    {
        sceneTransition.Play(TransitionEffectType.SlideStart);
        while (sceneTransition.IsProcessing)
        {
            yield return null;
        }
        
        curScene.OnExit();
        SceneManager.LoadScene(sceneType.ToString());
        yield return new WaitForSeconds(0.1f);
        curScene.OnEnter();
        
        sceneTransition.Play(TransitionEffectType.SlideEnd);
    }

    void IManager.Init()
    {
        
    }

    void IManager.Clear()
    {
        
    }
}