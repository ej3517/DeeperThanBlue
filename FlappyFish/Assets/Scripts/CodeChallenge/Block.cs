using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Block : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] protected Canvas canvas;

    public string type;         // Make this protected

    protected Transform currentTransform;
    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

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
        Debug.LogWarning("Begin drag");
        canvasGroup.blocksRaycasts = false;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogError("Dropped in box");
        if (eventData.pointerDrag != null)
        {
            currentTransform = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            //Block blockClass = eventData.pointerDrag.GetComponent<Block>();
            block.transform.SetParent(currentTransform);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -70,0); //TODO make size dynamic
            //Debug.LogError(blockClass.type.ToString());
        }
    }

    public virtual void Traverse()
    {
        //Traverse Tree. Has to be overloaded
        throw new System.NotImplementedException();
    }

}
