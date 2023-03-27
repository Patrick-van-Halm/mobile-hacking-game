using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BrowserUI : MonoBehaviour
{
    [SerializeField] private RectTransform _pagesHolder;
    [SerializeField] private GameObject _bookmarksPage;
    [SerializeField] private TMP_Text _addressField;
    
    private List<WebpageUI> _pages = new List<WebpageUI>();
    private GameObject _currentPage;

    private void Start()
    {
        OpenBookmarks();
        InstantiateWebpages();
    }

    public void OpenWebsite(Webpage page)
    {
        if (_currentPage) _currentPage.SetActive(false);
        var newPage = _pages.Find(p => p.HasURL(page.Link));
        _addressField.text = page.Link;
        _currentPage = newPage.gameObject;
        _currentPage.SetActive(true);
    }

    public void OpenBookmarks()
    {
        if (_currentPage) _currentPage.SetActive(false);
        _addressField.text = "Browser://Bookmarks";
        _currentPage = _bookmarksPage;
        _currentPage.SetActive(true);
    }

    public void InstantiateWebpages()
    {
        _pages.Clear();
        for (int i = _pagesHolder.childCount - 1; i >= 0; i--)
        {
            var page = _pagesHolder.GetChild(i);
            if (!page.GetComponent<WebpageUI>()) continue;
            Destroy(page.gameObject);
        }

        foreach (var page in BrowserManager.Instance.GetUnlockedWebsites())
        {
            var obj = Instantiate(page.PagePrefab, _pagesHolder);
            _pages.Add(obj.GetComponent<WebpageUI>());
            obj.SetActive(false);
        }
    }
}
