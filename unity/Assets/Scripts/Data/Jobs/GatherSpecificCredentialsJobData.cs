using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GatherSpecificCredentialsJobData : GatherCredentialsJobData
{
    [field: SerializeField] public string Username { get; protected set; }

    public GatherSpecificCredentialsJobData(string id, Person contactPerson, int minFactionReputation, int reward, int minLevel, bool completed, DateTime deadline, string targetDeviceId, string username) : base(id, contactPerson, minFactionReputation, reward, minLevel, completed, deadline, targetDeviceId)
    {
        Username = username;
    }

    public override void SocialEngineeringPasswordGained(DeviceUserData user)
    {
        if (user == null) return;
        if (user.Username != Username) return;
        DeviceManager.Instance.GetDevice(TargetDeviceId, data =>
        {
            if (!data.Users.Contains(user)) return;
            completed = true;
        });
    }

    public override string GetTitle()
    {
        return "Gather user credentials";
    }

    public override string GetDescription()
    {
        return "Gather credentials of a specific user on a target device.";
    }
}
