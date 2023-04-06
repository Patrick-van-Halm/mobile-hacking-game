using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTransactionsDTO
{
    [JsonProperty("incoming")]
    public List<GetTransactionDTO> Incoming { get; set; }

    [JsonProperty("outgoing")]
    public List<GetTransactionDTO> Outgoing { get; set; }
}

public class GetTransactionDTO
{
    [JsonProperty("public_id")]
    public string PublicId { get; set; }

    [JsonProperty("sender")]
    public GetPersonDTO Sender { get; set; }

    [JsonProperty("receiver")]
    public GetPersonDTO Receiver { get; set; }

    [JsonProperty("job")]
    public GetJobDTO Job { get; set; }

    [JsonProperty("amount")]
    public uint Amount { get; set; }
}

public class CreateTransactionDTO
{
    [JsonProperty("receiver_id")]
    public string ReceiverId { get; set; }
    
    [JsonProperty("sender_id")]
    public string SenderId { get; set; }

    [JsonProperty("job_id")]
    public string JobId { get; set; }

    [JsonProperty("amount")]
    public uint Amount { get; set; }
}