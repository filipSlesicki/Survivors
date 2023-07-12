using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class PoolManager
{
    static Dictionary<Component, ObjectPool<Component>> pools = new Dictionary<Component, ObjectPool<Component>>();

    public static T Get<T>(T obj) where T : Component
    {
        if(!pools.ContainsKey(obj))
        {
            pools.Add(obj, CreatePool(obj));
        }
        return pools[obj].Get() as T;
    }

    public static void Release(Component obj)
    {
        if (!pools.ContainsKey(obj))
        {
            Debug.LogError("Object pool doesn't exist");
            return;
        }
        pools[obj].Release(obj);
    }

    public static ObjectPool<Component> CreatePool(Component prefab)
    {
        return new ObjectPool<Component>(
          () =>  GameObject.Instantiate(prefab),
          OnGet,OnRelease,OnDestroy,false);
    }

    public static void ClearPools()
    {
        pools.Clear();
    }

    static void OnGet(Component obj)
    {
        obj.gameObject.SetActive(true);
    }

    static void OnRelease(Component obj)
    {
        obj.gameObject.SetActive(false);
    }
    static void OnDestroy(Component obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
}


