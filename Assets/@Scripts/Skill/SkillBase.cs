using System.Collections;
using UnityEngine;

public abstract class SkillBase : InitBase
{
    public UnitBase Owner { get; set; }
    public abstract Define.SkillType SkillType { get; }

    public Data.SkillData Data { get; private set; }
    public int CurLevel;

    public virtual void SetInfo(UnitBase owner, int skillID, int Level = 1)
    {
        this.Owner = owner;
        this.Data = Managers.Table.SkillDic[skillID];
        this.CurLevel = Level;
    }

    public virtual void LevelUp()
    {
        if (IsMaxLevel()) return;
        CurLevel++;
    }

    public bool IsMaxLevel()
    {
        return CurLevel == Data.maxLevel;
    }
    
    protected virtual T GenerateProjectile<T>() where T : Projectile
    {
        Projectile proj = Managers.Object.Spawn<Projectile>(Owner.GetPos());
        proj.SetInfo(Owner, Data.skillID, CurLevel);
        return proj as T;
    }
}