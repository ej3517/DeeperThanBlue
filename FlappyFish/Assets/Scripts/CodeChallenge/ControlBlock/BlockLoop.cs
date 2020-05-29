using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockLoop : Block
{
    private Block loopNext = null;
    private Block loopCond = null;

    //Transform start;
    //Transform end;


    //public Transform boxStart;
    //public Transform boxEnd;

    private void Awake()
    {
        type = "Loop";
        sizeHeight = 100;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //start = GameObject.Find("Start").GetComponent<Transform>();
        //end = GameObject.Find("End").GetComponent<Transform>();
    }

    RaycastHit hit;
    Ray ray;
    private enum Loopblock
    {
        Start,
        End,
        Default
    };

    static Loopblock lastState = Loopblock.Default;
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider.name + " " + (hit.collider.name == "BoxStart") + " " + (hit.collider.name == "BoxEnd"));
            if(hit.collider.name == "BoxStart")
            {
                lastState = Loopblock.Start;
            }
            else if(hit.collider.name == "BoxEnd")
            {
                lastState = Loopblock.End;
            }
        }
    }

    public override void OnDrop(PointerEventData eventData)
    //void Update()
    {
        if (eventData.pointerDrag != null)
        {
            Transform currentTransform = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            if(lastState == Loopblock.Start)
            {
                if(loopNext == null)
                {
                    loopNext = eventData.pointerDrag.GetComponent<Block>();
                    loopNext.SetAbove(this);
                    float _belowHeight = loopNext.GetSizeHeight();
                    block.transform.SetParent(currentTransform);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -( sizeHeight) / 2, 0); //TODO make size dynamic
                }
                else
                {
                    loopNext.OnDrop(eventData);
                }
            }
            else if(lastState == Loopblock.End)
            {
                //Same as Block.cs
                if (belowBlock == null)
                {
                    belowBlock = eventData.pointerDrag.GetComponent<Block>();
                    belowBlock.SetAbove(this);
                    float _belowHeight = belowBlock.GetSizeHeight();
                    block.transform.SetParent(currentTransform);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -(_belowHeight + sizeHeight) / 2, 0); //TODO make size dynamic
                                                                                                                                                                                                      //Debug.LogError(blockClass.type.ToString());
                }
                else
                {
                    belowBlock.OnDrop(eventData);
                }
            }
            else
            {
                Debug.LogError("LoopBlock Default state - Should never happen");
            }
        }
        Debug.Log("OnDrop");// throw new System.NotImplementedException();
    }

    public override IEnumerator Traverse(Transform Button)
    {
        //Sett button as child
        Button.SetParent(transform);        // Maybe not needed
        Button.localPosition = new Vector3(-200, 6);

        yield return new WaitForSeconds(1);
        //Debug.LogError("Turn after wait");
        if (belowBlock != null)
        {
            StartCoroutine(belowBlock.Traverse(Button));
        }
        else
        {
            Button.GetComponent<StartButton>().Restart();
        }
    }

    public override bool Validate()
    {
        //Call the next Node to check if a valid structure
        if (loopCond == null)
        {
            return false;       //TODO Update to check if LoopCond is valid;
        }
        else if(belowBlock == null)
        {
            return true;
        }
        else
        {
            return belowBlock.Validate();
        }
    }

    public override void SetBelow(Block _belowBlock, Block self)
    {
        if(self == belowBlock)
        {
            belowBlock = _belowBlock;
        }
        else if( self == loopNext)
        {
            loopNext = _belowBlock;
        }
        else
        {
            Debug.LogError("Unexpected SetBelow call - debug from loop");
        }
    }
}
