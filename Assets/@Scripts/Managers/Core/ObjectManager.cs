using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectManager : IManager
{
    public UnitPlayer Player { get; private set; }
    public HashSet<UnitMonster> Monsters { get; } = new HashSet<UnitMonster>();
    public HashSet<DropItem> Items { get; } = new HashSet<DropItem>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();
    public void Init()
    {
    }

    public void Clear()
    {
        Monsters.Clear();
    }

    private Coroutine temp = null;

    IEnumerator Check()
    {
        WaitForSeconds wait = new WaitForSeconds(1f);
        yield return wait;
        while (true)
        {
            int monCnt = Monsters.Count;
            int itemCnt = Items.Count;
            int projCnt= Projectiles.Count;
            int totalCnt= monCnt + itemCnt + projCnt;
            Debug.Log($"monsters:{monCnt} Items:{itemCnt}, Proj:{projCnt}, totalCount:{totalCnt},");
            yield return wait;
        }
    }
    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : ObjectBase
    {
        if (temp == null)
        {
            temp = Managers.StartCoroutine(Check());
        }
        System.Type type = typeof(T);

        if (type == typeof(UnitPlayer))
        {
            //플레이어 생성.
            GameObject go = Managers.Resource.Instantiate("Player");
            go.transform.position = position;
            UnitPlayer player = go.GetOrAddComponent<UnitPlayer>();
            player.Init();
            Player = player;
            Managers.Game.Grid.Add(player);
            Managers.Game.Grid.SetMaxSearchRadius(player.GetRadius());
            return player as T;
        }
        else if (type == typeof(UnitMonster))
        {
            GameObject go = Managers.Resource.Instantiate($"{prefabName}", pooling: true);
            UnitMonster monster = go.GetOrAddComponent<UnitMonster>();
            monster.Init();
            go.transform.position = position;
            monster.CellIndex = Managers.Game.Grid.GetCellIndex(position);
            Monsters.Add(monster);
            Managers.Game.Grid.Add(monster);
            Managers.Game.Grid.SetMaxSearchRadius(monster.GetRadius());
            return monster as T;
        }
        
        else if (type == typeof(Projectile))
        {
            GameObject go = Managers.Resource.Instantiate("FireBall", pooling: true);
            go.transform.position = position;

            Projectile pc = go.GetOrAddComponent<Projectile>();
            Projectiles.Add(pc);

            return pc as T;
        }
        else if (type == typeof(ExpItem))
        {
            GameObject go = Managers.Resource.Instantiate("ExpItem", pooling: true);
            go.transform.position = position;

            ExpItem item = go.GetOrAddComponent<ExpItem>();
            Items.Add(item);
            item.Init();
            
            //TODO.
            //스프라이트 붙이기.

            Managers.Game.Grid.Add(item);

            return item as T;
        }
        else if (type == typeof(MagnetItem))
        {
            GameObject go = Managers.Resource.Instantiate("Magnet", pooling: true);
            MagnetItem item = go.GetOrAddComponent<MagnetItem>();
            go.transform.position = position;
            Items.Add(item);
            Managers.Game.Grid.Add(item);

            return item as T;
        }
        
        else if (type == typeof(PotionItem))
        {
            GameObject go = Managers.Resource.Instantiate("Potion", pooling: true);
            PotionItem pc = go.GetOrAddComponent<PotionItem>();
            go.transform.position = position;
            Items.Add(pc);
            Managers.Game.Grid.Add(pc);

            return pc as T;
        }
        else if (type == typeof(BombItem))
        {
            GameObject go = Managers.Resource.Instantiate("Bomb", pooling: true);
            BombItem bc = go.GetOrAddComponent<BombItem>();
            go.transform.position = position;
            Items.Add(bc);
            Managers.Game.Grid.Add(bc);

            return bc as T;
        }
        return null;
    }

    public void Despawn<T>(T obj) where T : ObjectBase
    {
        System.Type type = typeof(T);

        if (type == typeof(UnitPlayer))
        {
            // ?
        }

        else if (type == typeof(UnitMonster))
        {
            Monsters.Remove(obj as UnitMonster);
            Managers.Resource.Destroy(obj.gameObject);
        }
        
        else if (type == typeof(DropItem))
        {
            Items.Remove(obj as DropItem);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(Projectile))
        {
            Projectiles.Remove(obj as Projectile);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }

    public void CollectAllItems()
    {
        foreach (var item in Items.Where(x=>x is ExpItem))
        {
            item.GetItem();
        }
    }
}