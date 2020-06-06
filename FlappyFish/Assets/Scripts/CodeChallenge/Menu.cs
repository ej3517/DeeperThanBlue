using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    public Transform pfblockItem;

    // Make sure to delete object if not dragged into the game area
    private void Awake()
    {
        ;// Debug.LogWarning("Button start");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogError("Settings Click");
        //Spawn instance
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ; //Set instance block at pointer
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
