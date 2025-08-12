using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class DropItem : ObjectBase
{
    private bool isEffectRunning = false;
    public override bool Init()
    {
        if (base.Init() == false)
        {
            //매번초기화.
            //...
            isEffectRunning = false;
            return false;
        }
        
        //한번만 초기화.
        //...
        isEffectRunning = false;
        return true;
    }

    public virtual void GetItem()
    {
        if (isEffectRunning) return;//자석아이템 연속으로 먹을경우, 버그발생을 방지.
            
        isEffectRunning = true;
        Managers.Game.Grid.Remove(this);
        MoveEffect();
    }
    public virtual void CompleteGetItem()
    {
        Managers.Object.Despawn(this);
    }

    private void MoveEffect()
    {
        Sequence seq = DOTween.Sequence();
        Vector3 dir = (GetPos() - Managers.Object.Player.GetPos()).normalized;
        Vector3 target = GetPos() + dir * 1f;
        seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.OutBack));
        seq.OnComplete(() =>
        {
            StartCoroutine(MoveToPlayer());
        });
    }
    private IEnumerator MoveToPlayer()
    {
        while (true)
        {
            float speed = 20.0f;
            var myPos = GetPos();
            var playerPos = Managers.Object.Player.GetPos();
            var newPos = Vector3.MoveTowards(myPos, playerPos, speed * Time.deltaTime);
            SetPos(newPos);

            if (Util.IsInDistance(playerPos, myPos, 0.4f))
            {
                CompleteGetItem();
                yield break;
            }

            yield return null;
        }
    }
}