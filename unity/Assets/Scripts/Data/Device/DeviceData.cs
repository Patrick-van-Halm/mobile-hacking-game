using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeviceData
{
	public static string GenerateRandomIPAddress()
	{
        byte[] ipChunks = new byte[4];
		for(int i = 0; i < ipChunks.Length; i++)
		{
			ipChunks[i] = (byte)UnityEngine.Random.Range(byte.MinValue, byte.MaxValue + 1);
		}
		return String.Join(".", ipChunks);
	}

	[field: SerializeField] public string Id { get; private set; }
	[field: SerializeField] public string IPAddress { get; private set; }
	[field: SerializeField] public string Name { get; private set; }
    [SerializeField] private List<PortData> _ports = new();
	[SerializeField] private List<DeviceUserData> _users = new();
	public IReadOnlyCollection<PortData> Ports => _ports;
	public IReadOnlyCollection<DeviceUserData> Users => _users;
	[SerializeField] private JObject _files;

	public DeviceData(string id, string ip, string name, JObject files, List<DeviceUserData> users, List<PortData> ports)
	{
		Id = id;
		IPAddress = ip;
		Name = name;
		_users = users;
		_ports = ports;
		_files = files;
	}

    public DeviceData(string ipAddress)
	{
		IPAddress = ipAddress;
		Name = "Computer";
		_ports = PortData.GenerateRandomPortList(4, .25f);
        _users = DeviceUserData.GenerateRandomUserList(1, 5);
    }
}
