using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum SceneType
    {
        Title,
        Lobby,
        Game,
    }
    public enum TransitionEffectType
    {
        FadeStart,
        FadeEnd,
        SlideStart,
        SlideEnd,
    }

    public enum UIEvent
    {
        Click,
        Preseed,
        PointerDown,
        PointerUp,
        BeginDrag,
        Drag,
        EndDrag,
    }

    public enum UnitState
    {
        Idle,
        Moving,
        Skill,
        Dead
    }

    public enum ObjectType
    {
        Unknown,
        Player,
        Monster,
        Projectile,
        Exp,
        Gold,
        Heal,
        Magnet,
        Bomb,
    }

    public enum SkillType
    {
        None = 0,
        FireBall = 100,
    }
}
