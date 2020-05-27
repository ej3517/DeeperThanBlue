using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bin : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    //private Transform currentObj;

    public Transform popupWindowPF;
    bool clickable = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.LogError("Delted Item");
            //currentObj = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            Block blockClass = eventData.pointerDrag.GetComponent<Block>();
            blockClass.DestroySelf();
            //Destroy(block);
            //Debug.LogError(blockClass.type.ToString());
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(clickable)
        {
            Debug.LogError("Clicked Item");
            Transform popupWindow = Instantiate(popupWindowPF);
            popupWindow.SetParent(transform);
            popupWindow.localPosition = new Vector3(-14, 20, -50);
            Text t = popupWindow.Find("Text").GetComponent<Text>();
            t.text = "Are you sure you would like to delete all blocks?";
        }
    }

    // public void OnMouseDown()
    // {
    //     Debug.LogError("Delted All items?");
    //     //
    // }

}


/*
 You can try this: objectB.position = parentTransform.InverseTransformPoint(objectC.position);

That will convert the objectC.position into the local space for the parentTransform, and then assign it to objectB
*/