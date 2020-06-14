using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockTurnRight : Block
{
    private void Awake()
    {
        SetType("TurnRight");
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override IEnumerator Traverse(Transform Button)
    {
        //Sett button as child
        Button.SetParent(transform);        // Maybe not needed
        Button.localPosition = new Vector3(-200, 6);

        Button.GetComponent<StartButton>().TurnRight();

        yield return new WaitForSeconds(Globals.CodeChallengeSpeed);
        //Debug.LogError("Turn after wait");
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
