using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;

public class Cell
{
    public HashSet<UnitMonster> Monsters { get; } = new HashSet<UnitMonster>();
    public HashSet<Projectile> Projectiles { get; } = new HashSet<Projectile>();
    public HashSet<DropItem> Items { get; } = new HashSet<DropItem>();
    public UnitPlayer Player { get; set; } = null;
}

public class GridManager
{
    private readonly Dictionary<Vector3Int, Cell> _cells = new Dictionary<Vector3Int, Cell>();

    public void Init()
    {
    }

    public void Clear()
    {
        _cells.Clear();
    }

    public void Add<T>(T obj) where T : ObjectBase
    {
        Vector3Int cellPos = GetCellIndex(obj.transform.position);
        Cell cell = GetCell(cellPos);

        switch (obj)
        {
            case UnitMonster monster:
                cell.Monsters.Add(monster);
                break;
            case Projectile proj:
                cell.Projectiles.Add(proj);
                break;
            case DropItem item:
                cell.Items.Add(item);
                break;
            case UnitPlayer player:
                cell.Player = player;
                break;
            default:
                Debug.LogError($"Unsupported type: {typeof(T)}");
                break;
        }
    }

    public void Remove<T>(T obj) where T : ObjectBase
    {
        Vector3Int cellPos = GetCellIndex(obj.GetPos());

        if (_cells.TryGetValue(cellPos, out Cell cell) == false)
        {
            Debug.LogWarning($"Cell not found at position {cellPos}");
            return;
        }

        switch (obj)
        {
            case UnitMonster monster:
                cell.Monsters.Remove(monster);
                break;
            case Projectile projectile:
                cell.Projectiles.Remove(projectile);
                break;
            case DropItem item:
                cell.Items.Remove(item);
                break;
            case UnitPlayer player:
                cell.Player = null;
                break;
            default:
                Debug.LogError($"Unsupported type: {typeof(T)}");
                break;
        }
    }

    public Cell GetCell(Vector3Int cellIndex)
    {
        if (_cells.TryGetValue(cellIndex, out Cell cell) == false)
        {
            cell = new Cell();
            _cells[cellIndex] = cell;
        }

        return cell;
    }

    public Vector3Int GetCellIndex(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        return new Vector3Int(x, y, 0);
    }

    public List<T> GatherObjects<T>(Vector3Int cellIndex,int searchRadius = 0) where T : ObjectBase
    {
        if (searchRadius == 0)
        {
            searchRadius = curSearchRadius;
        }
        List<T> objects = new List<T>();
        int minX = cellIndex.x - searchRadius;
        int maxX = cellIndex.x + searchRadius;
        int minY = cellIndex.y - searchRadius;
        int maxY = cellIndex.y + searchRadius;


        for (int x = minX; x <= maxX; x++)
        {
            for (int y = minY; y <= maxY; y++)
            {
                if (_cells.TryGetValue(new Vector3Int(x, y, 0), out Cell cell))
                {
                    if (typeof(T) == typeof(UnitMonster))
                        objects.AddRange(cell.Monsters.OfType<T>());
                    else if (typeof(T) == typeof(Projectile))
                        objects.AddRange(cell.Projectiles.OfType<T>());
                    else if (typeof(T) == typeof(DropItem))
                        objects.AddRange(cell.Items.OfType<T>());
                    else if(typeof(T)==typeof(UnitPlayer))
                    {
                        if (cell.Player != null)
                        {
                            objects.Add(cell.Player as T);
                            return objects;
                        }
                    }
                }
            }
        }

        return objects;
    }


    private int curSearchRadius = 1;
    public void SetMaxSearchRadius(float unitRadius)
    {
        //유닛반지름이 0.5미만이면, 1.
        //유닛반지름 1 미만이면, 2.
        //유닛반지름이 1.5미만이면, 3.

        if (unitRadius >= 1f)
        {
            curSearchRadius = 3;
        }
        else if (unitRadius >= 0.5f)
        {
            curSearchRadius = 2;
        }
        else
        {
            curSearchRadius = 1;
        }
    }
    
    

}