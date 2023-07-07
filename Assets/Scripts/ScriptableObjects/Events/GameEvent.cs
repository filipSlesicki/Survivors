using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent<T> : ScriptableObject
{
    private readonly List<IGameEventListener<T>> listeners = new List<IGameEventListener<T>>();

    public void Invoke(T data)
    {
        int count = listeners.Count;
        for (int i = 0; i < count; i++)
        {
            listeners[i].OnEventRaised(data);
        }
    }

    public void RegisterListener(IGameEventListener<T> newListener)
    {
        if(!listeners.Contains(newListener))
        {
            listeners.Add(newListener);
        }
    }

    public void UnregisterListener(IGameEventListener<T> listener)
    {
        listeners.Remove(listener);
    }

}
