using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PageListingUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _pageTitle;
    [SerializeField] private TMP_Text _pageUri;
    private Webpage _page;

    public void Initialize(Webpage page)
    {
        _page = page;
        _pageTitle.text = page.WebpageName;
        _pageUri.text = page.Link;
    }

    public void Goto()
    {
        BrowserManager.Instance.OpenWebsite(_page);
    }
}
