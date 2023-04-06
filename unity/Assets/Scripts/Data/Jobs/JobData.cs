using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class JobData
{
    // GatherCredentials - BlackHat/Whitehat
    // PenTest - WhiteHat
    // GatherEvidence - BlackHat
    // ChangeGrade - BlackHat
    // DenialOfService - BlackHat
    // GatherInfo - BlackHat

    [field: SerializeField] public string Id { get; private set; }
    [field: SerializeField] public Person ContactPerson { get; private set; }
    [field: SerializeField] public int MinFactionReputation { get; private set; }
    [field: SerializeField] public int CoinReward { get; private set; }
    [field: SerializeField] public int MinLevel { get; private set; }
    [field: SerializeField] public bool Completed { get; private set; }
    [field: SerializeField] public DateTime Deadline { get; private set; }

    public abstract bool Validate();
    public abstract string GetTitle();
    public abstract string GetDescription();

    public JobData(string id, Person contactPerson, int minFactionReputation, int reward, int minLevel, bool completed, DateTime deadline)
    {
        Id = id;
        ContactPerson = contactPerson;
        MinFactionReputation = minFactionReputation;
        CoinReward = reward;
        MinLevel = minLevel;
        Completed = completed;
        Deadline = deadline;
    }

    public void JobTaken()
    {
        // Todo: on job accepted
        JobManager.Instance.AcceptJob(this);
    }

    public void JobCancelled()
    {
        // Todo: take cancelled job reputation
        JobManager.Instance.CancelJob(this);
    }

    public void JobFinished()
    {
        // Todo: Give coin reward
        // Todo: Give reputation reward
    }
}
