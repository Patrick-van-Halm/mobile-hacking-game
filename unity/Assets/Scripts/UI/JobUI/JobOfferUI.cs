using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobOfferUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private TMP_Text _jobTitle;
    [SerializeField] private Button _viewBtn;

    public void Initialize(JobData job, JobUI jobMarketUI)
    {
        _coinsText.text = $"{job.CoinReward}";
        _jobTitle.text = job.GetTitle();

        _viewBtn.onClick.AddListener(() =>
        {
            jobMarketUI.ViewJob(job);
        });
    }
}
