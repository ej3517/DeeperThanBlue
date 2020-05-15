using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockForward : Block
{
    private void Awake()
    {
        type = "Forward";
        rectTransform = GetComponent<RectTransform>();
        Debug.LogWarning("Start, " + rectTransform.name);
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
