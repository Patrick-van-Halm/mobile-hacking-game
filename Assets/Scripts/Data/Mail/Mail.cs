using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Mail
{
    [field: SerializeField] public Person Sender { get; private set; }
    [field: SerializeField] public Person Receiver { get; private set; }
    [field: SerializeField] public string Subject { get; private set; }
    [field: SerializeField, TextArea] public string Body { get; private set; }
    [field: SerializeField] public JobData Job { get; private set; }
    [field: SerializeField] public bool HasRead { get; private set; }

    public Mail(Person sender, Person receiver, JobData job, string subject, string body, bool hasRead)
    {
        Sender = sender;
        Receiver = receiver;
        Job = job;
        Subject = subject;
        Body = body;
        HasRead = hasRead;
    }
}
