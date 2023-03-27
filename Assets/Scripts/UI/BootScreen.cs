using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BootScreen : MonoBehaviour
{
    [SerializeField] private GameObject _consoleLinePrefab;
    [SerializeField] private RectTransform _consoleLineHolder;
    [SerializeField] private ScrollRect _consoleScrollRect;
    private List<ConsoleLine> _consoleLines;

    private void Awake()
    {
        _consoleLines = new();
        _consoleLines.Add(new ConsoleLineText("Powering on system...", .02f));
        _consoleLines.Add(new ConsoleLineText("Performing self-test...", .02f));
        _consoleLines.Add(new ConsoleLineProgressFans(1, "<color=#00ff00>OK</color>", suffix: new("Testing memory.", .02f)));
        _consoleLines.Add(new ConsoleLineProgressFans(1, "<color=#00ff00>OK</color>", suffix: new("Testing CPU.", .02f)));
        _consoleLines.Add(new ConsoleLineProgressFans(2, "<color=#00ff00>OK</color>", suffix: new("Checking system configuration.", .02f)));
        _consoleLines.Add(new ConsoleLineText("\nLoading boot loader...", .02f));
        _consoleLines.Add(new ConsoleLineProgressDots(2, prefix: new("Verifying bootloader integrity", .02f)));
        _consoleLines.Add(new ConsoleLineText("Bootloader loaded successfully...", .02f));
        _consoleLines.Add(new ConsoleLineText("\nInitializing kernel...", .02f));
        _consoleLines.Add(new ConsoleLineProgressDots(2, prefix: new("Loading kernel modules", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(1, prefix: new("Mounting file systems", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(1, prefix: new("Setting up system resources", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(1, prefix: new("Configuring network interfaces", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(1, prefix: new("Starting system daemons", .02f)));
        _consoleLines.Add(new ConsoleLineText("\nStarting system services...", .02f));
        _consoleLines.Add(new ConsoleLineProgressDots(.6f, prefix: new("Starting syslogd", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(1.4f, prefix: new("Starting sshd", .02f)));
        _consoleLines.Add(new ConsoleLineProgressDots(.8f, prefix: new("Starting cupsd", .02f)));
        _consoleLines.Add(new ConsoleLineText("\nStarting desktop environment...", .02f));
        _consoleLines.Add(new ConsoleLineText("Starting file manager...", .02f));
        _consoleLines.Add(new ConsoleLineText("\nSystem initialization complete!", .02f));
    }

    private IEnumerator Start()
    {
        foreach (ConsoleLine consoleLine in _consoleLines)
        {
            var lineObj = Instantiate(_consoleLinePrefab, _consoleLineHolder);
            var textElement = lineObj.GetComponent<TMP_Text>();
            Canvas.ForceUpdateCanvases();
            _consoleScrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();

            yield return consoleLine.Execute(textElement);
        }
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
