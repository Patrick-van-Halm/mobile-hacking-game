using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

public class JobInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _jobTitle;
    [SerializeField] private TMP_Text _jobDescription;
    [SerializeField] private TMP_Text _jobContact;
    [SerializeField] private TMP_Text _jobCoinReward;
    [SerializeField] private TMP_Text _jobRepReward;
    private JobData _job;

    public void Initialize(JobData job)
    {
        _job = job;

        _jobTitle.text = job.GetTitle();
        _jobDescription.text = job.GetDescription();
        _jobContact.text = $"Contact: {job.ContactPerson}";
        _jobCoinReward.text = job.CoinReward.ToString();
    }

    public void AcceptJob()
    {
        // Todo: check if client can take job based of reputation
        _job.JobTaken();
    }
}
