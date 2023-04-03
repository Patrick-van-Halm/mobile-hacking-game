using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ReputationManager : SingletonMonoBehaviour<ReputationManager>
{
    [SerializeField] private string ApiPath;
    [SerializeField] private List<PlayerFactionReputation> factionReputations;
    
    public PlayerFactionReputation FactionReputation(string factionName) => factionReputations.Find(r => r.Faction == factionName);

    private IEnumerator Start()
    {
        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath.Replace(":id", Env.Instance.PlayerId)}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        factionReputations = JsonConvert.DeserializeObject<List<PlayerFactionReputation>>(request.downloadHandler.text);
    }
}
