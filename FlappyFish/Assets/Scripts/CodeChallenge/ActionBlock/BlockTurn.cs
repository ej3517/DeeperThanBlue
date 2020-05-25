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

    public override IEnumerator Traverse(Transform Button)
    {
        //Sett button as child
        Button.SetParent(transform);        // Maybe not needed
        Button.localPosition = new Vector3(-200, 6);

        yield return new WaitForSeconds(1);
        Debug.LogError("Turn after wait");
        if (belowBlock != null)
        {
            StartCoroutine(belowBlock.Traverse(Button));
        }
        else
        {
            Button.GetComponent<StartButton>().Restart();
        }
    }
}
