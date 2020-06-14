using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockVariable : Block
{
    public Transform leftVariable;
    public Transform rightInteger;

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
        string var = GetDDVar(leftVariable);
        int val = int.Parse(GetDDVar(rightInteger));
        Button.GetComponent<StartButton>().SetVar(var, val);

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
