using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Transaction
{
    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Person Sender { get; private set; }
    [field: SerializeField] public Person Receiver { get; private set; }
    [field: SerializeField] public JobData Job { get; private set; }
    [field: SerializeField] public uint Amount { get; private set; }

    public Transaction(string id, Person sender, Person receiver, JobData job, uint amount)
    {
        Id = id;
        Sender = sender;
        Receiver = receiver;
        Job = job;
        Amount = amount;
    }
}
