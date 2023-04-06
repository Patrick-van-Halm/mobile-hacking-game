using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OpenableWindow : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject _window;
    [SerializeField] private Image _taskbarBackgroundImage;
    public bool IsOpen => _window != null && _window.activeInHierarchy;

    public void OnPointerClick(PointerEventData eventData)
    {
        WindowManager.Instance.OpenWindow(this);
    }

    public void Close()
    {
        Open(false);
    }

    public void Open()
    {
        Open(true);
    }

    private void Open(bool isOpen)
    {
        if (!_window) return;
        _window.SetActive(isOpen);
        _taskbarBackgroundImage.color = new(_taskbarBackgroundImage.color.r, _taskbarBackgroundImage.color.g, _taskbarBackgroundImage.color.b, isOpen ? 1 : 0);
    }
}
