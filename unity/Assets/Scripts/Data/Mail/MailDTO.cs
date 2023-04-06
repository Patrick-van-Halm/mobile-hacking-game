using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMailDTO
{
    [JsonProperty("public_id")]
    public string PublicId { get; set; }

    [JsonProperty("sender")]
    public GetPersonDTO Sender { get; set; }

    [JsonProperty("receiver")]
    public GetPersonDTO Receiver { get; set; }

    [JsonProperty("job")]
    public GetJobDTO Job { get; set; }

    [JsonProperty("subject")]
    public string Subject { get; set; }

    [JsonProperty("body")]
    public string Body { get; set; }

    [JsonProperty("has_read")]
    public bool HasRead { get; set; }

    public Mail Mail => new(Sender.Person, Receiver.Person, Job?.ToJobData(), Subject, Body, HasRead);
}
