using UnityEngine;
using UnityEngine.Events;

//Data, Event, Unity Event Response
public abstract class GameEventListener<T, E, R> : MonoBehaviour,
    IGameEventListener<T> where E : GameEvent<T> where R : UnityEvent<T>
{
    [SerializeField] E gameEvent;
    [SerializeField] R response;

    private void OnEnable()
    {
        if (gameEvent == null)
            return;

        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        if (gameEvent == null)
            return;

        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T data)
    {
        response?.Invoke(data);
    }
}
