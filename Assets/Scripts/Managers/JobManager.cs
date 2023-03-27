using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobManager : SingletonMonoBehaviour<JobManager>
{
    [SerializeField] private JobDataEvent _onJobCreated;

    [SerializeField] private List<JobData> _jobs = new();
    public IReadOnlyCollection<JobData> Jobs => _jobs;

    [SerializeField] private List<JobData> _acceptedJobs = new();

    private IEnumerator Start()
    {
#if UNITY_EDITOR
        yield return null;
        var job = new GatherCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = true,
            MinReputation = 0,
            TargetDevice = DeviceManager.Instance.Devices.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);

        var device = DeviceManager.Instance.Devices.Random();
        job = new GatherSpecificCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = false,
            MinReputation = 0,
            TargetDevice = device,
            TargetUser = device.Users.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);

        job = new GatherCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = false,
            MinReputation = 0,
            TargetDevice = DeviceManager.Instance.Devices.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);

        device = DeviceManager.Instance.Devices.Random();
        job = new GatherSpecificCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = true,
            MinReputation = 0,
            TargetDevice = device,
            TargetUser = device.Users.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);

        job = new GatherCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = true,
            MinReputation = 0,
            TargetDevice = DeviceManager.Instance.Devices.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);

        device = DeviceManager.Instance.Devices.Random();
        job = new GatherSpecificCredentialsJobData()
        {
            CoinReward = 200,
            ContactPerson = "John",
            IsBlackHatActivity = false,
            MinReputation = 0,
            TargetDevice = device,
            TargetUser = device.Users.Random()
        };
        _jobs.Add(job);
        _onJobCreated.Invoke(job);
#endif
    }

    public void AcceptJob(JobData job)
    {
        _jobs.Remove(job);
        _acceptedJobs.Add(job);
    }

    public void CancelJob(JobData job)
    {
        _acceptedJobs.Remove(job);
        _jobs.Add(job);
    }
}
