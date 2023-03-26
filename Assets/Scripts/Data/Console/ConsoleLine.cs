using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

[Serializable]
public abstract class ConsoleLine {
    public abstract IEnumerator Execute(TMP_Text textElement);
}
