using System;
using UnityEngine;
using static Define;

// 게임한판이 끝나면 지워지는 임시적인 성격의 데이터를 관리.
public class GameDataManager : IManager
{
    private bool _init = false;
    void IManager.Init()
    {
        if (_init) return;
        _init = true;

    }
    void IManager.Clear()
    {
        Grid.Clear();
    }

    public Vector2 MoveDir { get; set; } = Vector2.zero;

    public GridManager Grid { get; set; } = new GridManager();
}