using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[Serializable]
public class ConsoleLineProgressDots : ConsoleLine
{
	private float _timeToWait;
	private float _timePerDot;
    private ConsoleLineText _prefix;
    private string _finishText;

    public ConsoleLineProgressDots(float timeToWait, float timePerDot = .25f, ConsoleLineText prefix = null, string finishText = "")
	{
		_timeToWait = timeToWait;
		_timePerDot = timePerDot;
		_prefix = prefix;
		_finishText = finishText;
	}

	public override IEnumerator Execute(TMP_Text textElement)
	{
        if (_prefix != null)
        {
            yield return _prefix.Execute(textElement);
        }

        int animTextStart = textElement.text.Length;
		string animText = "";
        float waited = 0;
        var prefixAnimText = textElement.text.Substring(0, animTextStart);
        while (waited < _timeToWait)
		{
			if (animText == "...") animText = "";
            animText += ".";
			textElement.text = prefixAnimText + animText;
            yield return new WaitForSeconds(_timePerDot);
			waited += _timePerDot;
		}
		if (_finishText != "") textElement.text = $"{prefixAnimText}{animText} {_finishText}";
    }
}
