using Newtonsoft.Json;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SocketManager : SingletonMonoBehaviour<SocketManager>
{
    private SocketIOUnity socket;

    private IEnumerator Start()
    {
        var uri = new Uri(Env.Instance.ApiUrl);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>()
            {
                { "token", Env.Instance.AuthKey }
            },
            Transport = SocketIOClient.Transport.TransportProtocol.WebSocket,
            EIO = 4,
            ConnectionTimeout = TimeSpan.FromSeconds(3)
        });
        socket.JsonSerializer = new NewtonsoftJsonSerializer();
        yield return socket.ConnectAsync();
    }

    public void On(string key, UnityAction<SocketIOResponse> callback)
    {
        socket.OnUnityThread(key, data => callback(data));
    }
}
