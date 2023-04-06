using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ChildSizeFitter : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        RectTransform biggestRect = null;
        for(int i = 0; i < _rectTransform.childCount; i++)
        {
            var child = _rectTransform.GetChild(i);
            if (child == null) continue;
            if (!child.gameObject.activeInHierarchy) continue;
            if (child.GetComponent<RectTransform>() == null) continue;
            var rect = child.GetComponent<RectTransform>();
            if (!biggestRect || rect.sizeDelta.y > biggestRect.sizeDelta.y) biggestRect = rect;
        }

        if (biggestRect) _rectTransform.sizeDelta = biggestRect.sizeDelta;
    }
}
