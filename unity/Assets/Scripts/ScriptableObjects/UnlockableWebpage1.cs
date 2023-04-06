using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Webpages/Unlockable Webpage")]
public class UnlockableWebpage : Webpage
{
    [field: SerializeField] public int MinReputation { get; private set; }
}
