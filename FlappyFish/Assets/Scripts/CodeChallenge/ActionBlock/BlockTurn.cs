using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTurn : Block
{
    private void Awake()
    {
        type = "Turn";
        rectTransform = GetComponent<RectTransform>();
        Debug.LogWarning("Start, " + rectTransform.name);
        canvasGroup = GetComponent<CanvasGroup>();
    }
}
