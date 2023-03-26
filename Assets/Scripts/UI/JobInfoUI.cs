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
    [SerializeField] private Button _jobAcceptBtn;
    [SerializeField] private Image _jobHatImg;
    [SerializeField] private Sprite _blackHatSprite;
    [SerializeField] private Sprite _whiteHatSprite;

    private JobData _job;

    public void Initialize(JobData job)
    {
        _job = job;

        _jobTitle.text = job.GetTitle();
        _jobDescription.text = job.GetDescription();
        _jobContact.text = $"Contact: {job.ContactPerson}";
        _jobCoinReward.text = job.CoinReward.ToString();

        if (job.IsBlackHatActivity) _jobHatImg.sprite = _blackHatSprite;
        else _jobHatImg.sprite = _whiteHatSprite;
    }

    public void AcceptJob()
    {
        _job.JobTaken();
    }
}
