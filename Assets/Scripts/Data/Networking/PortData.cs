using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PortData
{
    private static string[] _serviceNames = { 
        "SSH",
        "SQL",
        "FTP",
        "HTTP",
        "SSL",
        "Port Party",
        "MailMover",
        "CodeConnect",
        "GameGate",
        "FileForge",
        "TalkTime"
    };

    public static List<PortData> GenerateRandomPortList(int portCount, float openPortRatio)
    {
        List<PortData> portList = new List<PortData>();
        for(int i = 0; i < portCount; i++)
        {
            portList.Add(new(_serviceNames.Random(), (ushort)UnityEngine.Random.Range(0, ushort.MaxValue), UnityEngine.Random.Range(0, 100) < 50, openPortRatio > 0 && i < portCount * openPortRatio));
        }
        return portList;
    }

    [SerializeField] private string _serviceName;
    [SerializeField] private ushort _port;
    [SerializeField] private bool _isUPNP;
    [SerializeField] private bool _isOpen;

    public PortData(string serviceName, ushort port, bool isUPNP, bool isOpen)
    {
        _serviceName = serviceName;
        _port = port;
        _isUPNP = isUPNP;
        _isOpen = isOpen;
    }
}
