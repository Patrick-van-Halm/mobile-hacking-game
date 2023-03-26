using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeviceManager : SingletonMonoBehaviour<DeviceManager>
{
    [SerializeField] private List<DeviceData> _devices = new();
    public IReadOnlyCollection<DeviceData> Devices => _devices;

    private void Start()
    {
#if UNITY_EDITOR
        _devices.Add(new(DeviceData.GenerateRandomIPAddress()));
        _devices.Add(new(DeviceData.GenerateRandomIPAddress()));
        _devices.Add(new(DeviceData.GenerateRandomIPAddress()));
        _devices.Add(new(DeviceData.GenerateRandomIPAddress()));
        _devices.Add(new(DeviceData.GenerateRandomIPAddress()));
#endif
    }
}
