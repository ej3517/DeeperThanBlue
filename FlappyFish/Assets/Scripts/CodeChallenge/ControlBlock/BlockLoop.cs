using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlockLoop : Block
{
    private Block loopNext = null;

    public Transform dropDown;
    /*
    int menuIndex = dropDown.GetComponent<Dropdown>().value;
    List<Dropdown.OptionData> menuOptions = dropDown.GetComponent<Dropdown>().options;

    //get the string value of the selected index
    string value = menuOptions[menuIndex].text;
        Debug.Log(value);*/


    Transform loopTop = null;
    Transform loopBottom = null;

    Transform boxConnect;


    private void Awake()
    {
        SetType("Loop");
        sizeHeight = 110;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        loopTop = transform.Find("LoopTop").GetComponent<Transform>();
        loopBottom = transform.Find("LoopBottom").GetComponent<Transform>();
        boxConnect = transform.Find("BoxConnect").GetComponent<Transform>();
    }

    RaycastHit hit;
    Ray ray;
    private enum Loopblock
    {
        Start,
        End,
        Default
    };

    Loopblock lastState = Loopblock.Default;
    private void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log(hit.collider.name + " " + (hit.collider.name == "BoxStart") + " " + (hit.collider.name == "BoxEnd"));
            if (hit.collider.name == "BoxStart")
            {
                lastState = Loopblock.Start;
            }
            else if (hit.collider.name == "BoxEnd")
            {
                lastState = Loopblock.End;
            }
            else
            {
                lastState = Loopblock.Default;
            }
        }
        else
        {
            lastState = Loopblock.Default;
        }
        //Debug.Log("OnDrop " + lastState);         // Left for debug purposes



}

    private float loopContentSize = 10;
    public override void OnDrop(PointerEventData eventData)
    //void Update()
    {
        if (eventData.pointerDrag != null)
        {
            Transform currentTransform = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            if (lastState == Loopblock.Start)
            {
                if (loopNext == null)
                {
                    loopNext = eventData.pointerDrag.GetComponent<Block>();
                    loopNext.SetAbove(this);
                    float blockHeight = loopNext.GetSizeHeight();
                    block.transform.SetParent(loopTop);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(50, -(50 + blockHeight) / 2, 0); //TODO make size dynamic
                    aboveBlock?.BroadcastSize(loopNext.GetSizeHeightBelow() - loopContentSize, this);
                }
                else
                {
                    loopNext.OnDrop(eventData);
                }

                //Get code size
                float moveAmount = -loopNext.GetSizeHeightBelow() + loopContentSize;
                loopContentSize = loopNext.GetSizeHeightBelow();

                //Move the end down
                loopBottom.position = loopBottom.position + new Vector3(0, moveAmount, 0);
                updateBoxConnect();


            }
            else if (lastState == Loopblock.End || lastState == Loopblock.Default)       //Move Default to select where block goes by default
            {
                //Same as Block.cs
                if (belowBlock == null)
                {
                    belowBlock = eventData.pointerDrag.GetComponent<Block>();
                    belowBlock.SetAbove(this);
                    float blockHeight = belowBlock.GetSizeHeight();
                    block.transform.SetParent(loopBottom);
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -(50 + blockHeight) / 2, 0); //TODO make size dynamic
                    aboveBlock?.BroadcastSize(belowBlock.GetSizeHeightBelow(), this);
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
    }

    public override IEnumerator Traverse(Transform Button)
    {

        //Set child of Start
        Button.SetParent(loopTop);
        Button.localPosition = new Vector3(Button.localPosition.x, 0);

        int val = Button.GetComponent<StartButton>().GetVar(GetDDVar());
        //TODO: Display variable value

        yield return new WaitForSeconds(Globals.CodeChallengeSpeed);
        //Move check if variable is larger than 0
        if (val > 0)
        {
            StartCoroutine(loopNext.Traverse(Button));
        }
        else
        {
            Button.SetParent(loopBottom);
            Button.localPosition = new Vector3(Button.localPosition.x, 0);
            yield return new WaitForSeconds(Globals.CodeChallengeSpeed);

            if (belowBlock != null)
            {
                StartCoroutine(belowBlock.Traverse(Button));
            }
            else
            {
                Button.GetComponent<StartButton>().Restart();
            }
        }

        //Sett button as child

        
        //Debug.LogError("Turn after wait");

    }

    public override bool Validate()
    {
        bool inside, below;

        if (loopNext == null)
            inside = false;      //Change to allow empty scopes
        else
            inside = loopNext.Validate();

        if (belowBlock == null)
            below = true;
        else
            below = belowBlock.Validate();

        return inside && below;
    }

    public override void SetBelow(Block _belowBlock, Block self)
    {
        if (self == belowBlock)
        {
            belowBlock = _belowBlock;
        }
        else if (self == loopNext)
        {
            loopNext = _belowBlock;
        }
        else
        {
            Debug.LogError("Unexpected SetBelow call - debug from loop");
        }
    }

    public override void BroadcastSize(float size, Block self)
    {
        //if(self == loopCond)
        //{
        //    
        //}
        if (loopNext != null && self == loopNext)
        {
            loopContentSize += size;
            if (loopContentSize == 0)
            {
                loopContentSize = 10;
                size += 10;
            }
            loopBottom.position = loopBottom.position + new Vector3(0, -size, 0);
            updateBoxConnect();


        }
        aboveBlock?.BroadcastSize(size, this);
    }

    //public override float GetSizeHeight()
    //{
    //    float loopHeight = 10;
    //    if (loopNext != null)
    //    {
    //        loopHeight = loopNext.GetSizeHeightBelow();
    //    }
    //    return sizeHeight + loopHeight;
    //}

    public override float GetSizeHeightBelow()
    {
        float belowHeight = 0, loopHeight = 10, sizeHeight = 100;
        if (belowBlock != null)
        {
            belowHeight = belowBlock.GetSizeHeightBelow();
        }
        if (loopNext != null)
        {
            loopHeight = loopNext.GetSizeHeightBelow();
        }
        return belowHeight + loopHeight + sizeHeight;

    }

    private void updateBoxConnect()
    {
        float height = loopTop.position.y - loopBottom.position.y;
        boxConnect.GetComponent<RectTransform>().sizeDelta = new Vector3(50, height);
        //boxConnect.GetComponent<RectTransform>().localScale = new Vector3(50,height);
        boxConnect.localScale = new Vector3(50, height, 1);
        boxConnect.localPosition = new Vector3(-125, -height / 2 + 25, -1);
    }

    public override void blockRays(bool state)
    {
        canvasGroup.blocksRaycasts = state;
        loopTop.GetComponent<CanvasGroup>().blocksRaycasts = state;
        loopBottom.GetComponent<CanvasGroup>().blocksRaycasts = state;
        boxConnect.GetComponent<CanvasGroup>().blocksRaycasts = state;
    }
    private string GetDDVar()
    {
        int menuIndex = dropDown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = dropDown.GetComponent<Dropdown>().options;
        return menuOptions[menuIndex].text;
    }

}


