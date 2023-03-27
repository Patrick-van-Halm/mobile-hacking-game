using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableLinkUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PageListingUI _page;

    public void OnPointerClick(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
        _page.Goto();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorManager.Instance.SetPointer();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefault();
    }
}
