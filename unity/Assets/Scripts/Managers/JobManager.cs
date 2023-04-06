using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

public class JobManager : SingletonMonoBehaviour<JobManager>
{
    [SerializeField] private string ApiPath;
    [SerializeField] private JobDataEvent _onJobCreated;
    [SerializeField] private EmptyEvent _onJobListingChanged;

    [SerializeField] private List<JobData> _availableJobs = new();
    public IReadOnlyCollection<JobData> Jobs => _availableJobs;

    [SerializeField] private List<JobData> _acceptedJobs = new();

    private IEnumerator Start()
    {
        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        if(request.responseCode != 200) yield break;
        var jobs = JsonConvert.DeserializeObject<List<GetJobDTO>>(request.downloadHandler.text, new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
        });
        if(jobs == null) yield break;
        foreach(GetJobDTO j in jobs)
        {
            JobData data = j.ToJobData();
            if (data == null) continue;
            if (j.PerformingPlayerId == Env.Instance.PlayerId) _acceptedJobs.Add(data);
            else _availableJobs.Add(data);
        }
    }

    public void AcceptJob(JobData job)
    {
        _availableJobs.Remove(job);
        StartCoroutine(CoroAcceptJob(job));
        _onJobListingChanged?.Invoke();
    }

    private IEnumerator CoroAcceptJob(JobData job)
    {
        JObject changes = new();
        changes.Add("performing_player_id", Env.Instance.PlayerId);
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(changes));

        using UnityWebRequest request = UnityWebRequest.Put($"{Env.Instance.ApiUrl}{ApiPath}/{job.Id}", bytes);
        request.method = "PATCH";
        request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();

        if (request.responseCode != 200)
        {
            _availableJobs.Add(job);
            _onJobListingChanged?.Invoke();
            yield break;
        }
        _acceptedJobs.Add(job);
        _onJobListingChanged?.Invoke();
    }

    public void CancelJob(JobData job)
    {
        _acceptedJobs.Remove(job);
        _availableJobs.Add(job);
        _onJobListingChanged?.Invoke();
    }
}
