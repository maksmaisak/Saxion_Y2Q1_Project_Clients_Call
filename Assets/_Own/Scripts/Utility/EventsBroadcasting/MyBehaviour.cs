using UnityEngine;

/// A behaviour that can receive broadcast events.
public abstract class MyBehaviour : MonoBehaviour
{
    protected virtual void Awake()
    {
        EventsManager.instance.Add(this);
    }

    protected virtual void OnDestroy()
    {
        var manager = EventsManager.instance;
        if (manager) // In case it has been destroyed.
        {
            manager.Remove(this);
        }
    }
}