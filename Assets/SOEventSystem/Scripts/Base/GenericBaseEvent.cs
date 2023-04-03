using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GenericBaseEvent<T> : ScriptableObject
{
    protected List<GenericBaseEventListener<T>.Listener> _listeners = new();
    public UnityEvent<T> OnEvent { get; private set; } = new();

    public virtual void Invoke(T T1)
    {
        Debug.Log($"Invoking: {name}");
        foreach (GenericBaseEventListener<T>.Listener listener in _listeners) listener.Invoke(T1);
        OnEvent.Invoke(T1);
    }

    public void RegisterListener(GenericBaseEventListener<T>.Listener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GenericBaseEventListener<T>.Listener listener)
    {
        _listeners.Remove(listener);
    }
}

public abstract class GenericBaseEvent<T1, T2> : ScriptableObject
{
    protected List<GenericBaseEventListener<T1, T2>.Listener> _listeners = new();
    public UnityEvent<T1, T2> OnEvent { get; private set; } = new();

    public virtual void Invoke(T1 T1, T2 T2)
    {
        Debug.Log($"Invoking: {name}");
        foreach (GenericBaseEventListener<T1, T2>.Listener listener in _listeners) listener.Invoke(T1, T2);
        OnEvent.Invoke(T1, T2);
    }

    public void RegisterListener(GenericBaseEventListener<T1, T2>.Listener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GenericBaseEventListener<T1, T2>.Listener listener)
    {
        _listeners.Remove(listener);
    }
}