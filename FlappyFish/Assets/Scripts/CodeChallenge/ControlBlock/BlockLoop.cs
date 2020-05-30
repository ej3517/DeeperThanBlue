using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockLoop : Block
{
    private Block loopNext = null;
    private Block loopCond = null;

    Transform boxStart;
    Transform boxEnd;
    Transform boxConnect;


    private void Awake()
    {
        type = "Loop";
        sizeHeight = 110;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        boxStart = GameObject.Find("BoxStart").GetComponent<Transform>();
        boxEnd = GameObject.Find("BoxEnd").GetComponent<Transform>();
        boxConnect = GameObject.Find("BoxConnect").GetComponent<Transform>();
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
            else
            {
                lastState = Loopblock.Default;
            }
        }
    }

    private float loopContentSize = 10;
    public override void OnDrop(PointerEventData eventData)
    //void Update()
    {
        if (eventData.pointerDrag != null)
        {
            Transform currentTransform = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            if(lastState == Loopblock.Start)
            {
                float _shift = 0;
                if (loopNext == null)
                {
                    loopNext = eventData.pointerDrag.GetComponent<Block>();
                    loopNext.SetAbove(this);
                    float _belowHeight = loopNext.GetSizeHeight();
                    block.transform.SetParent(currentTransform);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -25, 0); //TODO make size dynamic
                    _shift = loopContentSize;
                }
                else
                {
                    loopNext.OnDrop(eventData);
                }

                //Get code size
                float sizeBelow = loopNext.GetSizeBelow();
                float moveAmount = sizeBelow - loopContentSize;

                //Move everything below down                                                                                                                                                                           //Debug.LogError(blockClass.type.ToString());
                if (belowBlock != null)
                {
                    belowBlock.UpdatePosition(-moveAmount+_shift);
                }
                //Move the end down
                boxEnd.position = boxEnd.position + new Vector3(0, -moveAmount+ _shift, 0);


            }
            else if(lastState == Loopblock.End || lastState == Loopblock.Default)       //Move Default to select where block goes by default
            {
                //Same as Block.cs
                if (belowBlock == null)
                {
                    belowBlock = eventData.pointerDrag.GetComponent<Block>();
                    belowBlock.SetAbove(this);
                    float _belowHeight = belowBlock.GetSizeHeight();
                    block.transform.SetParent(currentTransform);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -(_belowHeight + sizeHeight) / 2, 0); //TODO make size dynamic
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
        Debug.Log("OnDrop " + lastState);// throw new System.NotImplementedException();
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
        bool cond, inside, below;
        
        if (loopCond == null)
            return false;
        else
            cond = loopCond.Validate();

        if (loopNext == null)
            inside = true;      //Change to disallow empty scopes
        else
            inside = loopNext.Validate();

        if (belowBlock == null)
            below =  true;
        else
            below = belowBlock.Validate();

        return cond && inside && below;
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
