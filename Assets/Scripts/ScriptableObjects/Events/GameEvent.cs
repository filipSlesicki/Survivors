using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameEvent : ScriptableObject
{
    private readonly List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        int count = listeners.Count;
        for (int i = 0; i < count; i++)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener newListener)
    {
        if(!listeners.Contains(newListener))
        {
            listeners.Add(newListener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

}
