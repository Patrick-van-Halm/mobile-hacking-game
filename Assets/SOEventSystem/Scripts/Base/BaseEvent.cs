using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEvent : ScriptableObject
{
    protected List<BaseEventListener.Listener> _listeners = new();
    public UnityEvent OnEvent { get; private set; } = new();

    public void Invoke()
    {
        Debug.Log($"Invoking: {name}");
        foreach (BaseEventListener.Listener listener in _listeners) listener.Invoke();
        OnEvent.Invoke();
    }

    public void RegisterListener(BaseEventListener.Listener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(BaseEventListener.Listener listener)
    {
        _listeners.Remove(listener);
    }
}