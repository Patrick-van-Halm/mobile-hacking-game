using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    [SerializeField] private GameObject _blockerObject;

    private void Start()
    {
        foreach(Program program in ConsoleManager.Instance.ProgramList)
        {
            program.OnExecute.AddListener(BlockInputs);
            program.OnExecutionFinished.AddListener(AllowInputs);
        }
    }

    private void BlockInputs(Program program)
    {
        _blockerObject.SetActive(true);
    }

    private void AllowInputs(Program program)
    {
        _blockerObject.SetActive(false);
    }
}
