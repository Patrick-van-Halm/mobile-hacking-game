using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JobData
{
    // GatherCredentials - BlackHat/Whitehat
    // PenTest - WhiteHat
    // GatherEvidence - BlackHat
    // ChangeGrade - BlackHat
    // DenialOfService - BlackHat
    // GatherInfo - BlackHat

    public bool IsBlackHatActivity;
    public string ContactPerson;
    public int MinReputation;
    public int CoinReward;

    public abstract bool Validate();
    public abstract string GetTitle();
    public abstract string GetDescription();
    
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
