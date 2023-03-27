using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrowserManager : SingletonMonoBehaviour<BrowserManager>
{
    [SerializeField] private BookmarksUI _bookmarksUI;
    [SerializeField] private BrowserUI _browserUI;
    [SerializeField] private List<Webpage> _webpages;

    public void OpenWebsite(Webpage page)
    {
        _browserUI.OpenWebsite(page);
    }

    public IReadOnlyCollection<Webpage> GetUnlockedWebsites()
    {
        return _webpages;
    }
}
