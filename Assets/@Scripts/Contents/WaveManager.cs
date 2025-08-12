using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

//waveInfo 받아서, 어떤몬스터를, 얼마나, 자주, 어떤기믹으로 스폰할지를 처리.
public class WaveManager : MonoBehaviour
{
    public bool Stopped { get; set; } = false;
    float _spawnInterval = 0.01f;
    int _maxMonsterCount = 10000;
    Coroutine _coSpawn = null;

    void Start()
    {
        _coSpawn = StartCoroutine(CoSpawn());
    }

    IEnumerator CoSpawn()
    {
        while (true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    void TrySpawn()
    {
        if (Stopped)
            return;

        int monsterCount = Managers.Object.Monsters.Count;
        if (monsterCount >= _maxMonsterCount)
            return;

        Managers.Object.Spawn<UnitMonster>(Random.insideUnitCircle * 10f, 0, "Monster1");
    }
}