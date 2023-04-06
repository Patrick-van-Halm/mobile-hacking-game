using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Webpages/Webpage")]
public class Webpage : ScriptableObject
{
    [field: SerializeField] public string WebpageName { get; private set; }
    [field: SerializeField] public string Link { get; private set; }
    [field: SerializeField] public GameObject PagePrefab { get; private set; }

}
