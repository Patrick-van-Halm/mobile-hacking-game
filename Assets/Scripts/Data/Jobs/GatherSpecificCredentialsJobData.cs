using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherSpecificCredentialsJobData : GatherCredentialsJobData
{
    public DeviceUserData TargetUser;

    public override void SocialEngineeringPasswordGained(DeviceUserData user)
    {
        if (user == null) return;
        if (!TargetDevice.Users.Contains(user)) return;
        if (TargetUser != user) return;
        completed = true;
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
