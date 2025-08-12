using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static Define;

public abstract class ObjectBase : InitBase
{
    public abstract ObjectType ObjectType { get; }

    protected float Radius { get; set; } = 0.3f;

    public Vector3Int CellIndex { get; set; }
    public override bool Init()
    {
        if (base.Init() == false)
        {
            //매번초기화.
            //..
            return false;
        }
        
        //한번만 초기화.
        //...
        return true;
    }

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    protected bool IsCollision(ObjectBase other)
    {
        return Util.IsInDistance(GetPos(), other.GetPos(), GetRadius() + other.GetRadius());
    }
    public Vector3 GetPos()
    {
        return transform.position;
    }

    public float GetRadius()
    {
        return Radius;
    }

    protected void UpdateCell(ObjectBase obj)
    {
        if (obj is UnitMonster monster)
        {
            var newCellIndex = Managers.Game.Grid.GetCellIndex(GetPos());
            if (CellIndex != newCellIndex)
            {
                Managers.Game.Grid.GetCell(CellIndex).Monsters.Remove(monster);
                Managers.Game.Grid.GetCell(newCellIndex).Monsters.Add(monster);
                CellIndex = newCellIndex;
            }
        }
        else  if (obj is UnitPlayer player)
        {
            var newCellIndex = Managers.Game.Grid.GetCellIndex(GetPos());
            if (CellIndex != newCellIndex)
            {
                Managers.Game.Grid.GetCell(CellIndex).Player = null;
                Managers.Game.Grid.GetCell(newCellIndex).Player = player;
                CellIndex = newCellIndex;
            }
        }
        else  if (obj is Projectile proj)
        {
            var newCellIndex = Managers.Game.Grid.GetCellIndex(GetPos());
            if (CellIndex != newCellIndex)
            {
                Managers.Game.Grid.GetCell(CellIndex).Projectiles.Remove(proj);
                Managers.Game.Grid.GetCell(newCellIndex).Projectiles.Add(proj);
                CellIndex = newCellIndex;
            }
        }
        
        
    }
}