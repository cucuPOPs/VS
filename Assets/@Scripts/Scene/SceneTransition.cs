using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

using static Define;
public class SceneTransition : MonoBehaviour, IAnimStateUpdate
{
    [SerializeField] private Animator anim;
    // 애니메이션 상태 해시
    private readonly int fadeStart = Animator.StringToHash("FadeStart");
    private readonly int fadeEnd = Animator.StringToHash("FadeEnd"); 
    private readonly int slideStart = Animator.StringToHash("SlideStart");
    private readonly int slideEnd = Animator.StringToHash("SlideEnd");
    private readonly int none = Animator.StringToHash("None");

    public bool IsProcessing = false;
    private int curAnimStateHash = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Play(TransitionEffectType effectType)
    {
        IsProcessing = true;
        curAnimStateHash = effectType switch
        {
            TransitionEffectType.FadeStart => fadeStart,
            TransitionEffectType.FadeEnd => fadeEnd,
            TransitionEffectType.SlideStart => slideStart,
            TransitionEffectType.SlideEnd => slideEnd,
            _ => throw new Exception("Unknown transition effectType: " + effectType),
        };
        anim.Play(curAnimStateHash);
    }
    
    public void OnStateUpdate(int stateNameHash, float normalizedTime)
    {
        if(IsProcessing && curAnimStateHash == stateNameHash && normalizedTime >= 1.0f)
        {
            IsProcessing = false;
            if(curAnimStateHash == fadeEnd || curAnimStateHash == slideEnd)
            {
                anim.Play(none);
            }
        }
    }

}