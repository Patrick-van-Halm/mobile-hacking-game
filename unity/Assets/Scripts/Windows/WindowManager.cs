using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : SingletonMonoBehaviour<WindowManager>
{
    private OpenableWindow _currentWindow;

    public void OpenWindow(OpenableWindow window)
    {
        if(_currentWindow) _currentWindow.Close();
        _currentWindow = window;
        _currentWindow.Open();
    }
}