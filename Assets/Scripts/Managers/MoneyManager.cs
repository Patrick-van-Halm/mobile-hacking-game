using Newtonsoft.Json;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SocketIOClient;
using System;
using System.Text;

public class MoneyManager : SingletonMonoBehaviour<MoneyManager>
{
    [SerializeField] private string ApiPath;
    private List<Transaction> _incomingTransactions = new List<Transaction>();
    private List<Transaction> _outgoingTransactions = new List<Transaction>();

    public int Money => (int)(_incomingTransactions.Sum(t => t.Amount) + -_outgoingTransactions.Sum(t => t.Amount));

    [SerializeField] private IntEvent _onMoneyChanged;

    private IEnumerator Start()
    {
        SocketManager.Instance.On("MoneyReceived", WebsocketMoneyEarned);

        using UnityWebRequest request = UnityWebRequest.Get($"{Env.Instance.ApiUrl}{ApiPath}");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        yield return request.SendWebRequest();
        if (request.responseCode != 200) yield break;

        var data = JsonConvert.DeserializeObject<GetTransactionsDTO>(request.downloadHandler.text);
        if (data == null) yield break;

        foreach(var transaction in data.Incoming)
            _incomingTransactions.Add(new(transaction.PublicId, transaction.Sender?.Person, transaction.Receiver?.Person, transaction.Job?.ToJobData(), transaction.Amount));

        foreach(var transaction in data.Outgoing)
            _outgoingTransactions.Add(new(transaction.PublicId, transaction.Sender?.Person, transaction.Receiver?.Person, transaction.Job?.ToJobData(), transaction.Amount));
    }

    private void WebsocketMoneyEarned(SocketIOResponse response)
    {
        var data = response.GetValue<GetTransactionDTO>();
        _incomingTransactions.Add(new(data.PublicId, data.Sender.Person, data.Receiver.Person, data.Job?.ToJobData(), data.Amount));
        _onMoneyChanged?.Invoke(Money);
    }

    public void Spend(uint amount)
    {
        if (amount < 0) return;
        if (Money - amount < 0) return;

        StartCoroutine(CoroSpend(amount));
    }

    private IEnumerator CoroSpend(uint amount)
    {
        CreateTransactionDTO data = new()
        {
            Amount = amount
        };
        var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
        using UnityWebRequest request = UnityWebRequest.Put($"{Env.Instance.ApiUrl}{ApiPath}", bytes);
        request.SetRequestHeader("content-type", "application/json; charset=UTF-8");
        request.SetRequestHeader("Authorization", $"Bearer {Env.Instance.AuthKey}");
        request.method = "POST";
        yield return request.SendWebRequest();

        if (request.responseCode != 200) yield break;
        var transaction = JsonConvert.DeserializeObject<GetTransactionDTO>(request.downloadHandler.text);
        _outgoingTransactions.Add(new(transaction.PublicId, transaction.Sender.Person, transaction.Receiver.Person, transaction.Job?.ToJobData(), transaction.Amount));
        _onMoneyChanged?.Invoke(Money);
    }
}
