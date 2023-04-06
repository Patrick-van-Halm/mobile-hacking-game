using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataListManager : SingletonMonoBehaviour<DataListManager>
{
    [field: SerializeField] public ListScriptableObject UsernameList { get; private set; }
    [field: SerializeField] public ListScriptableObject PasswordList { get; private set; }
}
