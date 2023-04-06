using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "List", menuName = "Scriptable Objects/List string", order = 1)]
public class ListScriptableObject : ScriptableObject
{
    [SerializeField, TextArea] List<string> list = new List<string>();
    
    public string SelectRandomItem()
    {
        return list[Random.Range(0, list.Count)];
    }

    [ContextMenu("Paste List")]
    private void PasteList()
    {
        string value = GUIUtility.systemCopyBuffer;
        list.AddRange(value.Split('\n'));
    }
}
