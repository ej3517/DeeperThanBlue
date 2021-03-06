﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Block : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    private Canvas canvas;
    public void setCanvas(Canvas _canvas)
    {
        canvas = _canvas;
    }

    private string type;         // Make this protected
    protected void SetType(string _type)
    {
        type = _type;
    }
    public string GetType()
    {
        return type;
    }

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected bool topLayer = true;
    protected Block aboveBlock = null;
    protected Block belowBlock = null;     //Maybe not needed?

    protected float sizeHeight = 70;       // TODO: Make this dynamic

    private bool capArea = false;
    public void SetCapArea(bool state)
    {
        capArea = state;
    }

    /* protected Block instance;
     public Block()
     {
         instance = this;
     }*/
    private void Awake()
    {
    }

    //public defaultAwake()
    //{
    //    rectTransform = GetComponent<RectTransform>();
    //    Debug.LogWarning("Start, " + rectTransform.name);
    //    canvasGroup = GetComponent<CanvasGroup>();
    //    currentTransform = GetComponent<Transform>();
    //}

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.LogWarning("Begin drag");
        transform.position = transform.position + new Vector3(0, 0, 0.5f);
        blockRays(false);

        if (!topLayer)
        {
            //Detach and set as top layer
            Transform currentTransform = GetComponent<Transform>();
            currentTransform.transform.SetParent(GetParent());
            aboveBlock.BroadcastSize(-GetSizeHeightBelow(), this);
            aboveBlock.SetBelow(null, this);
            aboveBlock = null;
        }
        topLayer = true;
    }


    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if (capArea)
        {
            if (transform.position.x < 776)
            {
                transform.position = new Vector3(776, transform.position.y, transform.position.z);
            }
        }
        //Maybe move the object forward so doesn't overlap
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        transform.position = transform.position - new Vector3(0, 0, 0.5f);
        blockRays(true);
    }

    public virtual void blockRays(bool state)
    {
        canvasGroup.blocksRaycasts = state;
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Debug.LogWarning("OnPointerDown");
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        //Debug.LogError("Dropped in box");
        if ((eventData.pointerDrag != null) && (eventData.pointerDrag.GetComponent<Block>().type != "Start"))      // Weird bug where start can be dragged onto itself...
        {
            if (belowBlock == null)
            {
                Transform currentTransform = GetComponent<Transform>();
                Transform block = eventData.pointerDrag.GetComponent<Transform>();


                belowBlock = eventData.pointerDrag.GetComponent<Block>();
                belowBlock.SetAbove(this);
                float blockHeight = belowBlock.GetSizeHeight();
                block.transform.SetParent(currentTransform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -(blockHeight + sizeHeight) / 2, 0); //TODO make size dynamic
                                                                                                                                           //Debug.LogError(blockClass.type.ToString());
                BroadcastSize(belowBlock.GetSizeHeightBelow(), this);       //Make sure loops and statements increment size
            }
            else
            {
                belowBlock.OnDrop(eventData);
            }
        }
    }

    private Transform GetParent()
    {
        if (aboveBlock != null)
        {
            //Debug.LogError(type);
            return aboveBlock.GetParent();
        }
        else
        {
            Transform currentTransform = GetComponent<Transform>();
            return currentTransform.transform.parent.transform;
        }
    }

    public void SetAbove(Block _aboveBlock)
    {
        aboveBlock = _aboveBlock;
        topLayer = false;
    }

    public virtual void SetBelow(Block _belowBlock, Block self)
    {
        belowBlock = _belowBlock;
    }

    public virtual IEnumerator Traverse(Transform Button)
    {
        //Traverse Tree. Has to be overloaded
        throw new System.NotImplementedException();
    }

    public virtual bool Validate()
    {
        //Call the next Node to check if a valid structure
        if (belowBlock == null)
        {
            return true;
        }
        else
        {
            return belowBlock.Validate();
        }
    }


    public virtual void DestroySelf()
    {
        if (belowBlock != null)
        {
            belowBlock.DestroySelf();
        }
        Destroy(transform.gameObject);
    }

    public virtual float GetSizeHeight()
    {
        //Debug.Log("returning height " + sizeHeight);
        return sizeHeight;
    }

    public virtual float GetSizeHeightBelow()
    {
        //float loopSize
        if (belowBlock == null)
        {
            return sizeHeight;
        }
        else
        {
            return sizeHeight + belowBlock.GetSizeHeightBelow();
        }

    }

    public virtual void UpdatePosition(float verticleDistance)
    {
        transform.position = transform.position + new Vector3(0, verticleDistance, 0);
        if (belowBlock != null)
        {
            belowBlock.UpdatePosition(verticleDistance);
        }
    }

    public virtual void BroadcastSize(float size, Block self)
    {
        aboveBlock?.BroadcastSize(size, this);
    }

    protected static string GetDDVar(Transform dropDown)
    {
        int menuIndex = dropDown.GetComponent<Dropdown>().value;
        List<Dropdown.OptionData> menuOptions = dropDown.GetComponent<Dropdown>().options;
        return menuOptions[menuIndex].text;
    }
}

