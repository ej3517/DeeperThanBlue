using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockForward : Block
{
    private void Awake()
    {
        SetType("Forward");
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override IEnumerator Traverse(Transform Button)
    {
        //Sett button as child
        Button.SetParent(transform);        // Maybe not needed
        Button.localPosition = new Vector3(Button.localPosition.x, 6);

        yield return new WaitForSeconds(Globals.CodeChallengeSpeed);
        if(Button.GetComponent<StartButton>().Forward())
        {
            if (belowBlock != null)
            {
                StartCoroutine(belowBlock.Traverse(Button));
            }
            else
            {
                Button.GetComponent<StartButton>().End();
            }
        }
    }

}
