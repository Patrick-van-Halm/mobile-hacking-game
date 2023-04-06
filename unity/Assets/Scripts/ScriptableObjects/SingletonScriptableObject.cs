using System.Linq;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    [field: SerializeField] public bool Enabled { get; private set; } = true;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                SingletonScriptableObject<T>[] assets = Resources.LoadAll<SingletonScriptableObject<T>>("Scriptable Objects/").Where(a => a.Enabled).ToArray();
                if (assets == null || assets.Length == 0)
                    throw new System.Exception($"No instance of type: {typeof(T).Name} found!");
                else if (assets.Length > 1)
                    Debug.LogWarning($"Multiple instances of type: {typeof(T).Name} found!");
                _instance = assets[0] as T;
            }
            return _instance;
        }
    }
}