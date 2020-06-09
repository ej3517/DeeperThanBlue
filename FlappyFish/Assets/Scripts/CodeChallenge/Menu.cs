using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
{
    public Transform codingArea;
    public Transform pfblockItem;
    [SerializeField] private Canvas canvas;
    private Transform block;

    // Make sure to delete object if not dragged into the game area
    private void Awake()
    {
        ;// Debug.LogWarning("Button start");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        block = Instantiate(pfblockItem);
        block.GetComponent<Block>().setCanvas(canvas);
        //set position of current object
        block.SetParent(transform.parent);
        block.position = transform.position;

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        // Check if in correct area. If not delete...
        eventData.pointerDrag = block.gameObject;
        if (block.position.x < 630)
        {
            block.GetComponent<Block>().DestroySelf();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        block.GetComponent<Block>().OnBeginDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Check if correct place else delete
        block.GetComponent<Block>().OnEndDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (block.position.x > 630)
        {
            block.SetParent(codingArea);
            block.GetComponent<Block>().SetCapArea(true);
        }
        block.GetComponent<Block>().OnDrag(eventData);
    }

}
