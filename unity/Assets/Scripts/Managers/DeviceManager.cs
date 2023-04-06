using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DeviceManager : SingletonMonoBehaviour<DeviceManager>
{
    [SerializeField] private string ApiPath;
    [SerializeField] private List<DeviceData> _devices = new();
    public IReadOnlyCollection<DeviceData> Devices => _devices;

    private IEnumerator Start()
    {
        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        if (request.responseCode != 200) yield break;

        var data = JsonConvert.DeserializeObject<List<APIData>>(request.downloadHandler.text, new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
        });
        if (data == null) yield break;

        foreach(var device in data)
        {
            List<DeviceUserData> users = new();
            foreach (var userData in device.Data.Users)
            {
                users.Add(new(userData.Username, userData.Password, false));
            }

            List<PortData> ports = new();
            foreach (var portData in device.Data.Services)
            {
                ports.Add(new(portData.Name, portData.Port, portData.UPNP, portData.Open));
            }
            _devices.Add(new DeviceData(device.PublicId, device.IP, device.Name, device.Data.Files, users, ports));
        }
    }

    public void GetDevice(string id, UnityAction<DeviceData> callback)
    {
        var device = _devices.Find(d => d.Id == id);
        if (device == null)
            StartCoroutine(Get(id, callback));
        else
            callback(device);
    }

    private IEnumerator Get(string id, UnityAction<DeviceData> callback)
    {
        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath}/{id}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        var data = JsonConvert.DeserializeObject<APIData>(request.downloadHandler.text, new JsonSerializerSettings
        {
            DateTimeZoneHandling = DateTimeZoneHandling.Local,
        });
        if (data == null) yield break;

        List<DeviceUserData> users = new();
        foreach(var userData in data.Data.Users)
        {
            users.Add(new(userData.Username, userData.Password, false));
        }

        List<PortData> ports = new();
        foreach(var portData in data.Data.Services)
        {
            ports.Add(new(portData.Name, portData.Port, portData.UPNP, portData.Open));
        }
        var device = new DeviceData(data.PublicId, data.IP, data.Name, data.Data.Files, users, ports);
        _devices.Add(device);

        callback(device);
    }

    private class APIData
    {
        [JsonProperty("public_id")] public string PublicId { get; set; }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("ip")] public string IP { get; set; }
        [JsonProperty("data")] public APIDeviceData Data { get; set; }
    }

    private class APIDeviceData
    {
        [JsonProperty("users")]
        public List<APIDeviceUserData> Users { get; set; }

        [JsonProperty("files")]
        public JObject Files { get; set; }

        [JsonProperty("services")]
        public List<APIServiceData> Services { get; set; }
    }

    private class APIDeviceUserData
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

    private class APIServiceData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("port")]
        public ushort Port { get; set; }

        [JsonProperty("isUPNP")]
        public bool UPNP { get; set; }

        [JsonProperty("isOpen")]
        public bool Open { get; set; }
    }
}
