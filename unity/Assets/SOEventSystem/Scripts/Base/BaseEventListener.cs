using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseEventListener : MonoBehaviour
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
        public BaseEvent BaseEvent;
        public UnityEvent Event;

        public void Invoke()
        {
            Event.Invoke();
        }
    }
}