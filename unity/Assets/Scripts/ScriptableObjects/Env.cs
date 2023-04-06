using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Env")]
public class Env : SingletonScriptableObject<Env>
{
    [field: SerializeField] public string ApiUrl { get; private set; }
    [field: SerializeField] public string AuthKey { get; private set; }
    [field: SerializeField] public string PlayerId { get; private set; }

    public void Authenticate(string playerId, string key)
    {
        if (playerId == null || playerId.Length != 21) return;
        if (key == null || key.Length != 128) return;
        PlayerId = playerId;
        AuthKey = key;
    }
}
