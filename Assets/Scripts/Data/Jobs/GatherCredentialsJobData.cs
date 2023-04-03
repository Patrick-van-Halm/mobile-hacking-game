using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GatherCredentialsJobData : JobData
{
    [field: SerializeField] public string TargetDeviceId { get; protected set; }

    protected bool completed = false;

    public GatherCredentialsJobData(string id, Person contactPerson, int minFactionReputation, int reward, int minLevel, bool completed, DateTime deadline, string targetDeviceId) : base(id, contactPerson, minFactionReputation, reward, minLevel, completed, deadline)
    {
        TargetDeviceId = targetDeviceId;
    }

    public override bool Validate()
    {
        return completed;
    }

    public void PasswordsDecrypted(DeviceData device)
    {
        if (device == null) return;
        if (device.Id != TargetDeviceId) return;
        completed = true;
    }

    public virtual void SocialEngineeringPasswordGained(DeviceUserData user)
    {
        if (user == null) return;
        DeviceManager.Instance.GetDevice(TargetDeviceId, data =>
        {
            if (!data.Users.Contains(user)) return;
            completed = true;
        });
    }

    public override string GetTitle()
    {
        return "Gather any credentials";
    }

    public override string GetDescription()
    {
        return "Gather any kind of user credentials on a target device.";
    }

    public class APIData
    {
        [JsonProperty("target_device")] public string TargetDevice;
    }
}
