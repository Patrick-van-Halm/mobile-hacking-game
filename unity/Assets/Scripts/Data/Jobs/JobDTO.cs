using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetJobDTO
{
    [JsonProperty("public_id")]
    public string PublicId { get; set; }

    [JsonProperty("contact_person")]
    public GetPersonDTO ContactPerson { get; set; }

    [JsonProperty("performing_player_id")]
    public string PerformingPlayerId { get; set; }

    [JsonProperty("job_type")]
    public string JobType { get; set; }

    [JsonProperty("payout_amount")]
    public int PayoutAmount { get; set; }

    [JsonProperty("min_level")]
    public int MinLevel { get; set; }

    [JsonProperty("min_faction_reputation")]
    public int MinFactionReputation { get; set; }

    [JsonProperty("data")]
    public JObject Data { get; set; }

    [JsonProperty("deadline")]
    public DateTime Deadline { get; set; }

    [JsonProperty("completed")]
    public bool Completed { get; set; }

    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    public JobData ToJobData()
    {
        switch(JobType)
        {
            case "UserCredentials":
                return new GatherSpecificCredentialsJobData(PublicId, ContactPerson.Person, MinFactionReputation, PayoutAmount, MinLevel, Completed, Deadline, Data["target_device"].Value<string>(), Data["username"].Value<string>());

            default:
                return null;
        }
    }
}
