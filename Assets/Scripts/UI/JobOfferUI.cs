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
    [SerializeField] private Image _hatImg;
    [SerializeField] private Sprite _blackHatSprite;

    public void Initialize(JobData job, JobUI jobMarketUI)
    {
        _coinsText.text = $"{job.CoinReward}";
        _jobTitle.text = job.GetTitle();

        if (job.IsBlackHatActivity) _hatImg.sprite = _blackHatSprite;

        _viewBtn.onClick.AddListener(() =>
        {
            jobMarketUI.ViewJob(job);
        });
    }
}
