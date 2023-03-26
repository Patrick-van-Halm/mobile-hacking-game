using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T[] assets = Resources.LoadAll<T>("Scriptable Objects/");
                if (assets == null || assets.Length == 0)
                    throw new System.Exception($"No instance of type: {typeof(T).Name} found!");
                else if (assets.Length > 1)
                    Debug.LogWarning($"Multiple instances of type: {typeof(T).Name} found!");
                _instance = assets[0];
            }
            return _instance;
        }
    }
}