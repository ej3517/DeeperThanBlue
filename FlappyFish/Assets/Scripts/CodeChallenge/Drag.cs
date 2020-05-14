using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        Debug.LogWarning("Start, " + rectTransform.name);
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.LogWarning("Begin drag");
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta/canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogWarning("OnPointerDown");
    }

    public void OnDrop(PointerEventData eventData)
    {
      //  throw new System.NotImplementedException();
    }
}


/*
Here's what I did to do the snap back to position if the item wasn't dropped on a slot.

1. Create a Vector3 variable, let's call it defaultPos. In the Start class in the DragDrop script, set this variable in its position. So defaultPos = GetComponent<RectTransform>().localPosition;

2. Put a public bool in the DragDrop script, let's call it droppedOnSlot

3. From the ItemSlot script, set the bool you made to true. So: eventData.pointerDrag.GetComponent<DragDrop>().droppedOnSlot = true

4. In the DragDrop script, create a coroutine. Put a "yield return new WaitforEndofFrame()" line. After that line, you may then check droppedOnSlot, if it's false (meaning none of the ItemSlot scripts activated), you can then reset the position. Start this coroutine in the OnEndDrag class.

5. Don't forget to put a code inside the OnBeginDrag class to set droppedOnSlot back to false.
*/
