using UnityEngine;

public interface IAnimStateEnter
{
    void OnStateEnter(int stateNameHash);
}

public interface IAnimStateUpdate
{
    void OnStateUpdate(int stateNameHash, float normalizedTime);
}

public interface IAnimStateExit
{
    void OnStateExit(int stateNameHash);
}

public class AnimStateHandler : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        var t = animator.GetComponent<IAnimStateEnter>();
        if (t != null) t.OnStateEnter(stateInfo.shortNameHash);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);
        var t = animator.GetComponent<IAnimStateUpdate>();
        if (t != null) t.OnStateUpdate(stateInfo.shortNameHash, stateInfo.normalizedTime);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        var t = animator.GetComponent<IAnimStateExit>();
        if (t != null) t.OnStateExit(stateInfo.shortNameHash);
    }
}