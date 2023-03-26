using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Scriptable Objects/Programs/Nmap")]
public class PortScanner : Program
{
    [SerializeField] private GameObject _actionPrefab;

    private void OnEnable()
    {
        _consoleLines.Clear();
        _consoleLines.Add(new ConsoleLineProgressDots(2, prefix: new("PortSc4n initializing", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(5, prefix: new ConsoleLineText("Scanning for open ports on target device", .02f), finishText: "[<color=#00ff00>OK</color>]"));
        _consoleLines.Add(new ConsoleLineProgressFans(5, "<color=#00ff00>OK</color>", suffix: new ConsoleLineText("Scanning for open ports on target device.", 0)));
        _consoleLines.Add(new ConsoleLineText("Scanning <color=#00ff00>success</color>!"));
    }

    public override void ExecutionFinished()
    {
        ConsoleManager.Instance.InstantiateProgramAction(_actionPrefab);
        base.ExecutionFinished();
    }
}
