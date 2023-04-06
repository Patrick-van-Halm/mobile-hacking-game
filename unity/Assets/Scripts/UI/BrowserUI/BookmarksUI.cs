using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookmarksUI : MonoBehaviour
{
    [SerializeField] private GameObject _pageListingPrefab;
    [SerializeField] private RectTransform _pageListingHolder;

    private void OnEnable()
    {
        InitializeBookmarkedPages();
    }

    private void InitializeBookmarkedPages()
    {
        for (int i = _pageListingHolder.childCount - 1; i >= 0; i--)
        {
            var page = _pageListingHolder.GetChild(i);
            if (!page.GetComponent<PageListingUI>()) continue;
            Destroy(page.gameObject);
        }

        foreach (var page in BrowserManager.Instance.GetUnlockedWebsites())
        {
            var obj = Instantiate(_pageListingPrefab, _pageListingHolder);
            var listing = obj.GetComponent<PageListingUI>();
            listing.Initialize(page);
        }
    }
}
