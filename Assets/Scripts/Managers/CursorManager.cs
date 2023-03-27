using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : SingletonMonoBehaviour<CursorManager>
{
    [SerializeField] private Texture2D _defaultCursor;
    [SerializeField] private Texture2D _pointerCursor;

    void Start()
    {
        SetDefault();
    }

    public void SetDefault()
    {
        ChangeCursor(_defaultCursor);
    }

    public void SetPointer()
    {
        ChangeCursor(_pointerCursor);
    }

    private void ChangeCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, new(0,0), CursorMode.Auto);
    }
}
