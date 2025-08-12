using System.Collections;
using Data;
using UnityEngine;

public abstract class Projectile : ObjectBase
{
    private UnitBase owner;
    private int skillID;
    private int skillLevel;
    public override Define.ObjectType ObjectType => Define.ObjectType.Projectile;

    public virtual void SetInfo(UnitBase owner, int skillID, int skillLevel)
    {
        this.owner = owner;
        this.skillID = skillID;
        this.skillLevel = skillLevel;

        StartCoroutine(CoDestroy());
    }
    
    IEnumerator CoDestroy()
    {
        var data = Managers.Table.SkillDic[skillID];
        yield return new WaitForSeconds(5f);
        DestroyProjectile();
    }
    public void DestroyProjectile()
    {
        Managers.Game.Grid.Remove(this);
        Managers.Object.Despawn(this);
    }

    public void OnDamaged(UnitBase target)
    {
        target.OnDamaged(owner, skillID, skillLevel);
    }
}