using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailManager : SingletonMonoBehaviour<MailManager>
{
    [SerializeField] private MailEvent _onMailReceived;

    private void Start()
    {
#if UNITY_EDITOR
        
#endif
    }

    public void SendMail(Mail mail)
    {

    }

    private void OnMailReceived(Mail mail)
    {
        _onMailReceived.Invoke(mail);
    }
}
