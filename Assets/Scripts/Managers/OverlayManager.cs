using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayManager : SingletonMonoBehaviour<OverlayManager>
{
    [SerializeField] private GameObject _overlay;
    [SerializeField] private GameObject _jobInfoOverlay;
    private GameObject _activeOverlay;

    private void ShowOverlay(bool shown)
    {
        _activeOverlay.SetActive(shown);
        _overlay.SetActive(shown);
    }

    public void ShowJobInfo()
    {
        _activeOverlay = _jobInfoOverlay;
        ShowOverlay(true);
    }

    public void HideOverlay()
    {
        ShowOverlay(false);
    }
}
