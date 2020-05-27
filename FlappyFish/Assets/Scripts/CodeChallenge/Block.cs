using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] protected Canvas canvas;

    protected string type;         // Make this protected

    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected bool topLayer = true;
    protected Block aboveBlock = null;
    protected Block belowBlock = null;     //Maybe not needed?


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
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        if(!topLayer)
        {
            //Detach and set as top layer
            Transform currentTransform = GetComponent<Transform>();
            currentTransform.transform.SetParent(GetParent());
            aboveBlock.SetBelow(null);
            aboveBlock = null;
        }
        topLayer = true;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        //Debug.LogWarning("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.LogError("Dropped in box");
        if ((eventData.pointerDrag != null) && (eventData.pointerDrag.GetComponent<Block>().type != "Start") )      // Weird bug where start can be dragged onto itself...
        {
            if (belowBlock == null)
            {
                Transform currentTransform = GetComponent<Transform>();
                Transform block = eventData.pointerDrag.GetComponent<Transform>();


                belowBlock = eventData.pointerDrag.GetComponent<Block>();
                belowBlock.SetAbove(this);
                block.transform.SetParent(currentTransform);
                eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -70, 0); //TODO make size dynamic
                                                                                                                                                                     //Debug.LogError(blockClass.type.ToString());
            }
            else
            {
                belowBlock.OnDrop(eventData);
            }
        }
    }

    private Transform GetParent()
    {
        if (!topLayer)
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

    public void SetBelow(Block _belowBlock)
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
        if(belowBlock==null)
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
        if(belowBlock != null)
        {
            belowBlock.DestroySelf();
        }
        Destroy(transform.gameObject);
    }

}
