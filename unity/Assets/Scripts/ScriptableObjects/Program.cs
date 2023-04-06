using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Program : ScriptableObject
{
    protected List<ConsoleLine> _consoleLines = new();

    [field: SerializeField] public UnityEvent<Program> OnExecute { get; private set; } = new();
    [field: SerializeField] public UnityEvent<Program> OnExecutionFinished { get; private set; } = new();
    public bool HasConsoleLines => _consoleLines.Count > 0;
    public IReadOnlyCollection<ConsoleLine> ConsoleLines => _consoleLines;

    public void Execute()
    {
        OnExecute.Invoke(this);
    }

    public virtual void ExecutionFinished()
    {
        OnExecutionFinished.Invoke(this);
    }
}
