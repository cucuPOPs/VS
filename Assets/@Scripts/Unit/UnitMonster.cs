using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using static Define;

public class UnitMonster : UnitBase
{
    public override ObjectType ObjectType => ObjectType.Monster;

    private Coroutine DamageCo = null;
    private bool isColliding = false;
    
    public override bool Init()
    {
        if (base.Init() == false)
        {
            //매번초기화.
            //..
            return false;
        }
            
        //한번만 초기화.
        Speed = 1f;
        Radius = 0.3f;
        return true;
    }

    void Update()
    {
        if (Hp <= 0) return;
        Move();
        UpdateAnimation();
        CheckCollision();
    }

    void Move()
    {
        
        var player = Managers.Object.Player;
        Vector3 dir = (player.GetPos() - this.GetPos()).normalized;
        var newPos = GetPos() + dir * (Speed * Time.deltaTime);
        SetPos(newPos);
        UpdateFlip(dir);
        UpdateCell(this);
    }

    void UpdateAnimation()
    {
        if (isColliding)
        {
            ChangeAnimState(AnimState.Idle);
            return;
        }
        else
        {
            ChangeAnimState(AnimState.Move);    
        }
    }
    
    void CheckCollision()
    {
        var player = Managers.Game.Grid.GatherObjects<UnitPlayer>(CellIndex).FirstOrDefault();
        if (player == null)
        {
            // 플레이어가 없으면 충돌 상태를 초기화하고 코루틴을 중지
            if (DamageCo != null) StopCoroutine(DamageCo);
            isColliding = false;
            return;
        }
        else
        {
            isColliding = IsCollision(player);
            if (DamageCo == null)
            {
                DamageCo = StartCoroutine(DoDamage());
            }
        }
    }

    IEnumerator DoDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            Managers.Object.Player.OnDamaged(this,0,1);
        }
    }

    protected override void OnDead()
    {
        base.OnDead();
        if (DamageCo != null) StopCoroutine(DamageCo);
        
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f);
        seq.OnComplete(() =>
        {
            Managers.Object.Spawn<ExpItem>(GetPos());
            Managers.Object.Despawn(this);
        });
    }
}