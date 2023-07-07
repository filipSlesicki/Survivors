using UnityEngine;

[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEvent : GameEvent<Void>
{
    public void Raise() => Invoke(new Void());
}
