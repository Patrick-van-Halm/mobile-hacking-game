using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPersonDTO
{
    [JsonProperty("public_id")]
    public string PublicId { get; set; }

    [JsonProperty("player_id")]
    public string PlayerId { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("faction")]
    public string Faction { get; set; }

    [JsonProperty("email_address")]
    public string EmailAddress { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public Person Person => new(PublicId, PlayerId, Faction, Name, EmailAddress);
}
