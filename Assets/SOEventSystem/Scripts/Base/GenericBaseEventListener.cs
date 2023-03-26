using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GenericBaseEventListener<T> : MonoBehaviour
{
    [SerializeField] private List<Listener> listeners = new List<Listener>();

    private void OnEnable()
    {
        foreach (Listener listener in listeners) listener.BaseEvent.RegisterListener(listener);
    }

    private void OnDisable()
    {
        foreach (Listener listener in listeners) listener.BaseEvent.UnregisterListener(listener);
    }

    [Serializable]
    public class Listener
    {
        public GenericBaseEvent<T> BaseEvent;
        public UnityEvent<T> Event;

        public void Invoke(T t)
        {
            Event.Invoke(t);
        }
    }
}

public abstract class GenericBaseEventListener<T1, T2> : MonoBehaviour
{
    [SerializeField] private List<Listener> listeners = new List<Listener>();

    private void OnEnable()
    {
        foreach (Listener listener in listeners) listener.BaseEvent.RegisterListener(listener);
    }

    private void OnDisable()
    {
        foreach (Listener listener in listeners) listener.BaseEvent.UnregisterListener(listener);
    }

    [Serializable]
    public class Listener
    {
        public GenericBaseEvent<T1, T2> BaseEvent;
        public UnityEvent<T1, T2> Event;

        public void Invoke(T1 T1, T2 T2)
        {
            Event.Invoke(T1, T2);
        }
    }
}