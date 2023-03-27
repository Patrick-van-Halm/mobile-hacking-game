using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebpageUI : MonoBehaviour
{
    [SerializeField] private Webpage _page;
    public bool HasURL(string url) => _page.Link == url;
}
