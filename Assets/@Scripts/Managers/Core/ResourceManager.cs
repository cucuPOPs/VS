using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

public class ResourceManager : IManager
{
    Dictionary<Type, Dictionary<string, Object>> _caches = new Dictionary<Type, Dictionary<string, Object>>();

    private bool _init = false;
    
    public void Init()
    {
        if (_init) return;
        _init = true;
        
        PreLoad();
    }
    public void Clear()
    {
        _caches.Clear();
        _init = false;
    }

    private void PreLoad()
    {
        Object[] objects = Resources.LoadAll("PreLoad");
        foreach (var obj in objects)
        {
            if (obj is SpriteAtlas atlas)
            {
                Sprite[] sprites = new Sprite[atlas.spriteCount];
                atlas.GetSprites(sprites);
                foreach (var sprite in sprites)
                {
                    AddCache(sprite);
                }
            }
            else
            {
                AddCache(obj);
            }
        }
    }

    private void AddCache(Object obj)
    {
        Type type = obj.GetType();
        string key = obj.name;
        if (!_caches.ContainsKey(type)) _caches[type] = new Dictionary<string, Object>();
        _caches[type][key] = obj;
    }


    public T Load<T>(string key) where T : Object
    {
        //캐시에서 로드
        Type type = typeof(T);
        var dic = _caches[type];
        if (dic.TryGetValue(key, out var value))
        {
            return (T)value;
        }

        //없으면 Resources에서 로드
        T resource = Resources.Load<T>(key);
        if (resource != null)
        {
            AddCache(resource);
            return resource;
        }

        Debug.LogError($"Failed to load resource: {key}");
        return null;
    }

    public GameObject Instantiate(string key, Transform parent = null, bool pooling = false)
    {
        GameObject prefab = Load<GameObject>(key);
        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab : {key}");
            return null;
        }

        if (pooling)
            return Managers.Pool.Pop(prefab);

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        if (Managers.Pool.Push(go))
            return;

        Object.Destroy(go);
    }
}
