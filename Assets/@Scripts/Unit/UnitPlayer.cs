using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Analytics;
using UnityEngine;
using static Define;

public class UnitPlayer : UnitBase
{
    public override ObjectType ObjectType => ObjectType.Player;

    private float searchBonus = 0.5f; 
    public override bool Init()
    {
        if (base.Init() == false)
        {
            //매번초기화.
            //..
            return false;
        }
        
        //한번만 초기화.
        Speed = 3;
        Skill.AddSkill(100);
        return true;
    }


    void Update()
    {
        if (Hp <= 0) return;
        Move();
        UpdateAnimation();
        CollectEnv();
    }

    private void Move()
    {
        if (Managers.Game.MoveDir == Vector2.zero) return;
        
        Vector3 newPos = GetPos() + (Vector3)Managers.Game.MoveDir * (Speed * Time.deltaTime);
        SetPos(newPos);
        UpdateFlip(Managers.Game.MoveDir);
        UpdateCell(this);
    }

    private void UpdateAnimation()
    {
        if (Managers.Game.MoveDir != Vector2.zero)
        {
            ChangeAnimState(AnimState.Move);
        }
        else
        {
            ChangeAnimState(AnimState.Idle);
        }
    }

    private void CollectEnv()
    {
        List<DropItem> items = Managers.Game.Grid.GatherObjects<DropItem>(CellIndex);

        foreach (var item in items)
        {
            switch (item.ObjectType)
            {
                default:
                    float dist = item.GetRadius() + GetRadius() + searchBonus;
                    bool isCollision = Util.IsInDistance(item.GetPos(),  this.GetPos(), dist);
                    if (isCollision)
                    {
                        item.GetItem();
                    }
                    break;
            }
        }
    }


    public void Healing(float healAmount)
    {
        //
    }
}