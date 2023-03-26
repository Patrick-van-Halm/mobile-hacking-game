using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherCredentialsJobData : JobData
{
    public DeviceData TargetDevice;
    protected bool completed = false;

    public override bool Validate()
    {
        return completed;
    }

    public void PasswordsDecrypted(DeviceData device)
    {
        if (device == null) return;
        if (device != TargetDevice) return;
        completed = true;
    }

    public virtual void SocialEngineeringPasswordGained(DeviceUserData user)
    {
        if (user == null) return;
        if (!TargetDevice.Users.Contains(user)) return;
        completed = true;
    }

    public override string GetTitle()
    {
        return "Gather any credentials";
    }

    public override string GetDescription()
    {
        return "Gather any kind of user credentials on a target device.";
    }
}
