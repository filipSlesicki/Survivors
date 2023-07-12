using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public static class PoolManager
{
    static Dictionary<string, ObjectPool<Component>> pools = new Dictionary<string, ObjectPool<Component>>();

    public static T Get<T>(T obj) where T : Component
    {
        if(!pools.ContainsKey(obj.gameObject.name))
        {
            pools.Add(obj.gameObject.name, CreatePool(obj));
        }
        return pools[obj.gameObject.name].Get() as T;
    }

    public static void Release(Component obj)
    {
        if (!pools.ContainsKey(obj.gameObject.name))
        {
            Debug.LogError("Object pool doesn't exist");
            return;
        }
        pools[obj.gameObject.name].Release(obj);
    }

    public static ObjectPool<Component> CreatePool(Component prefab)
    {
        return new ObjectPool<Component>(
          OnCreate, OnGet, OnRelease, OnDestroy, false) ;

        Component OnCreate()
        {
            Component created = GameObject.Instantiate(prefab);
            created.gameObject.name = prefab.name;
            return created;
        }
        void OnGet(Component obj)
        {
            obj.gameObject.SetActive(true);
        }

        void OnRelease(Component obj)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public static void ClearPools()
    {
        pools.Clear();
    }


    static void OnDestroy(Component obj)
    {
        GameObject.Destroy(obj.gameObject);
    }
}


