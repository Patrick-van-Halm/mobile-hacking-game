using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobUI : MonoBehaviour
{
    [SerializeField] private GameObject _jobOfferPrefab;
    [SerializeField] private RectTransform _jobOfferHolder;
    [SerializeField] private TMP_Text _moneyText;

    [SerializeField] private JobInfoUI _jobInfoUI;

    [SerializeField] private JobDataEvent _onJobCreated;
    [SerializeField] private JobDataEvent _onJobRemoved;
    [SerializeField] private IntEvent _onMoneyChanged;

    private void Awake()
    {
        _onJobCreated.OnEvent.AddListener(OnJobCreated);
        _onJobRemoved.OnEvent.AddListener(OnJobRemoved);
        _onMoneyChanged.OnEvent.AddListener(OnMoneyChanged);
    }

    private void OnMoneyChanged(int money)
    {
        _moneyText.text = money.ToString();
    }

    private void OnJobRemoved(JobData job)
    {
        throw new NotImplementedException();
    }

    private void OnJobCreated(JobData job)
    {
        var obj = Instantiate(_jobOfferPrefab, _jobOfferHolder);
        var offerUI = obj.GetComponent<JobOfferUI>();
        offerUI.Initialize(job, this);
    }

    public void ViewJob(JobData job)
    {
        _jobInfoUI.Initialize(job);
        OverlayManager.Instance.ShowJobInfo();
    }

    private void OnEnable()
    {
        for(int i = _jobOfferHolder.childCount - 1; i >= 0; i--)
        {
            Destroy(_jobOfferHolder.GetChild(i).gameObject);
        }

        foreach (var job in JobManager.Instance.Jobs) OnJobCreated(job);
        _moneyText.text = MoneyManager.Instance.Money.ToString();
    }
}