using Newtonsoft.Json;
using SocketIOClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MailManager : SingletonMonoBehaviour<MailManager>
{
    [SerializeField] private string ApiPath;
    [SerializeField] private List<Mail> mails = new List<Mail>();
    [SerializeField] private MailEvent _onMailReceived;

    private IEnumerator Start()
    {
        SocketManager.Instance.On("EmailReceived", WebsocketEmailReceived);

        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        if (request.responseCode != 200) yield break;

        var data = JsonConvert.DeserializeObject<List<GetMailDTO>>(request.downloadHandler.text);
        if (data == null) yield break;

        foreach(var mail in data)
        {
            mails.Add(mail.Mail);
        }
    }

    private void WebsocketEmailReceived(SocketIOResponse socketResponse)
    {
        var emailData = socketResponse.GetValue<GetMailDTO>();
        OnMailReceived(emailData.Mail);
    }

    public void SendMail(Mail mail)
    {

    }

    private void OnMailReceived(Mail mail)
    {
        mails.Add(mail);
        _onMailReceived.Invoke(mail);
    }
}
