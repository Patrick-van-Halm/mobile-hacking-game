using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[Serializable]
public class ConsoleLineProgressFans : ConsoleLine
{
	private float _timeToWait;
	private float _timePerSwitch;
	private ConsoleLineText _prefix;
	private ConsoleLineText _suffix;
	private char[] _animChars = { '|', '/', '-', '\\', '|', '/', '-', '\\' };
	private string _finishText;

	public ConsoleLineProgressFans(float timeToWait, string finishText = "", float timePerSwitch = .25f, ConsoleLineText prefix = null, ConsoleLineText suffix = null)
	{
		_timeToWait = timeToWait;
		_timePerSwitch = timePerSwitch;
		_prefix = prefix;
		_suffix = suffix;
		_finishText = finishText;
    }

	public override IEnumerator Execute(TMP_Text textElement)
	{
		if(_prefix != null)
		{
			yield return _prefix.Execute(textElement);
			textElement.text += " ";
		}

		int animTextStart = textElement.text.Length;
		int animTextEnd = textElement.text.Length + 3;

        if (_suffix != null)
        {
            textElement.text += "[" + _animChars[0] + "] ";
            yield return _suffix.Execute(textElement);
        }

        float waited = 0;
		int charIndex = 0;

        var prefixAnimText = textElement.text.Substring(0, animTextStart);
        var suffixAnimText = textElement.text.Substring(animTextEnd);
        while (waited < _timeToWait)
		{
			if (charIndex >= _animChars.Length) charIndex = 0;
			textElement.text = $"{prefixAnimText}[{_animChars[charIndex].ToString()}]{suffixAnimText}";
			charIndex++;
            yield return new WaitForSeconds(_timePerSwitch);
			waited += _timePerSwitch;
		}

        if(_finishText != "") textElement.text = $"{prefixAnimText}[{_finishText}]{suffixAnimText}";
    }
}
