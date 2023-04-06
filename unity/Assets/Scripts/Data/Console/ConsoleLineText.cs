using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[Serializable]
public class ConsoleLineText : ConsoleLine
{
	private string _text;
    private float _characterWriteSpeed;

	public ConsoleLineText(string text, float characterWriteSpeed = .1f)
	{
        _text = text;
        _characterWriteSpeed = characterWriteSpeed;
	}

	public override IEnumerator Execute(TMP_Text textElement)
	{
        if(_characterWriteSpeed == 0)
        {
            textElement.text += _text;
            yield break;
        }

        var instantTagsMatches = Regex.Matches(_text, "<(.*?)>");
        for(int i = 0; i < _text.Length; i++)
        {
            Match foundMatch = null;
            foreach(Match match in instantTagsMatches)
            {
                if (match.Index != i) continue;
                
                foundMatch = match;
                break;
            }

            if(foundMatch != null)
            {
                textElement.text += foundMatch.Value;
                i += foundMatch.Length - 1;
                continue;
            }

            char character = _text[i];
            textElement.text += character;
            yield return new WaitForSeconds(_characterWriteSpeed);
        }
    }
}
