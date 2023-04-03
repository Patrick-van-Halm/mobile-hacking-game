using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerFactionReputation
{
    [JsonProperty("faction")]
    [field:SerializeField] 
    public string Faction { get; set; }
    [JsonProperty("reputation")]
    [field: SerializeField]
    public int Reputation { get; set; }
}
