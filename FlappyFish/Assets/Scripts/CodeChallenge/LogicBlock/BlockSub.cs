using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSub : Block
{

    public Transform leftVar;
    public Transform midVar;
    public Transform rightVar;

    private void Awake()
    {
        SetType("Variable");
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override IEnumerator Traverse(Transform Button)
    {
        Button.SetParent(transform);
        Button.localPosition = new Vector3(Button.localPosition.x, 6);

        string left = GetDDVar(leftVar);
        int midVal = Button.GetComponent<StartButton>().GetVar(GetDDVar(midVar));
        int right = int.Parse(GetDDVar(rightVar));

        Button.GetComponent<StartButton>().SetVar(left, midVal - right);

        yield return new WaitForSeconds(Globals.CodeChallengeSpeed);
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
