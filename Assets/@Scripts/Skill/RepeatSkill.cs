using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    #region CoSkill

    Coroutine _coSkill;
    private WaitForSeconds wait;
    public virtual void Activate()
    {
        if (_coSkill != null)
            StopCoroutine(_coSkill);
        wait = new WaitForSeconds(Data.cooltime[CurLevel - 1]);
        _coSkill = StartCoroutine(CoSkill());
    }

    protected virtual IEnumerator CoSkill()
    {
        yield return wait;
        while (true)
        {
            DoSkill();
            yield return wait;
        }
    }

    protected abstract void DoSkill();

    public override void LevelUp()
    {
        base.LevelUp();
        wait = new WaitForSeconds(Data.cooltime[CurLevel - 1]);
    }

    #endregion
}