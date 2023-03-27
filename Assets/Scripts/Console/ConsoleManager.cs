using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleManager : SingletonMonoBehaviour<ConsoleManager>
{
    [Header("Console Line")]
    [SerializeField] private GameObject _consoleLinePrefab;
    [SerializeField] private Transform _consoleLineHolder;
    [SerializeField] private ScrollRect _consoleScrollRect;
    [SerializeField] private string _consoleLinePrefix;

    [Header("Programs")]
    [SerializeField] private List<Program> _programList = new();
    [SerializeField] private RectTransform _actionArea;

    [Header("Target")]
    [SerializeField] private DeviceDataEvent _onTargetChanged;
    public DeviceData SelectedTarget { get; private set; }


    public IReadOnlyCollection<Program> ProgramList => _programList;

    private void Start()
    {
        foreach(Program program in _programList)
        {
            program.OnExecute.AddListener(OnProgramExecution);
        }
    }

    private void OnProgramExecution(Program program)
    {
        if (!program.HasConsoleLines) return;
        StartCoroutine(PrintLines(program));
    }

    private IEnumerator PrintLines(Program program)
    {
        foreach(ConsoleLine consoleLine in program.ConsoleLines)
        {
            var lineObj = Instantiate(_consoleLinePrefab, _consoleLineHolder);
            var textElement = lineObj.GetComponent<TMP_Text>();
            textElement.text += _consoleLinePrefix;
            Canvas.ForceUpdateCanvases();
            _consoleScrollRect.verticalNormalizedPosition = 0;
            Canvas.ForceUpdateCanvases();
            
            yield return consoleLine.Execute(textElement);
        }
        program.ExecutionFinished();
    }

    public GameObject InstantiateProgramAction(GameObject prefab)
    {
        if (_actionArea.childCount == 1) Destroy(_actionArea.GetChild(0));
        return Instantiate(prefab, _actionArea);
    }

    public void SelectTarget(DeviceData target)
    {
        SelectedTarget = target;
        _onTargetChanged.Invoke(target);
    }
}
