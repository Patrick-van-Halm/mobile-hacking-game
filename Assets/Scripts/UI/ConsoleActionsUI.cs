using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleActionsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _targetIPAddress;
    [SerializeField] private TMP_Text _targetDeviceName;
    [SerializeField] private Image _targetDeviceIcon;
    [SerializeField] private DeviceDataEvent _onTargetChanged;

    private void Awake()
    {
        _onTargetChanged.OnEvent.AddListener(UpdateTargetInfo);
    }

    private void OnEnable()
    {
        UpdateTargetInfo(ConsoleManager.Instance.SelectedTarget);
    }

    private void UpdateTargetInfo(DeviceData target)
    {
        _targetIPAddress.text = target?.IPAddress ?? "";
        _targetDeviceName.text = target?.Name ?? "";
        if (target == null) _targetDeviceIcon.enabled = false;
        else
        {

        }
    }
}
