using System;
using UnityEngine;
using static Define;

public abstract class UnitBase : ObjectBase
{
    public Animator Anim { get; private set; }
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;
    public float Speed { get; set; } = 1.0f;

    protected enum AnimState
    {
        None,
        Idle,
        Move,
        Death
    }


    protected AnimState _animState { get; private set; } = AnimState.None;
    public SkillManager Skill { get; private set; }
    public override bool Init()
    {
        //매번초기화.
        if (base.Init() == false)
        {
            ChangeAnimState(AnimState.Idle);
            Hp = MaxHp;
            return false;
        }
        
        //한번만 초기화.
        Anim = Util.FindChild<Animator>(gameObject);
        Skill = gameObject.GetOrAddComponent<SkillManager>();
        Skill.Init(this);
        Hp = MaxHp;
        return true;
    }


    protected void ChangeAnimState(AnimState newState)
    {
        if (_animState == newState) return;
        switch (newState)
        {
            case AnimState.Idle:
                Anim.Play("Idle");
                break;
            case AnimState.Move:
                Anim.Play("Move");
                break;
            case AnimState.Death:
                Anim.Play("Death");
                break;
        }

        _animState = newState;
    }
    protected void UpdateFlip(Vector2 _moveDir)
    {
        Anim.transform.localScale = _moveDir.x < 0 ? Vector3.one : new Vector3(-1, 1, 1);
    }

    public virtual void OnDamaged(ObjectBase attacker, int skillID, int skillLevel)
    {
        if (Hp <= 0)
            return;
        
        int damage = Managers.Table.SkillDic[skillID].damage[skillLevel - 1];    
        
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        Skill.StopSkills();
        ChangeAnimState(AnimState.Death);
        Managers.Game.Grid.Remove(this);
    }
}