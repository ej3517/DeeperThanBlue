using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Bin : MonoBehaviour, IDropHandler, IPointerDownHandler
{
    //private Transform currentObj;

    public Transform popupWindow;
    public Transform codingArea;

    bool clickable = true;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            Debug.Log("Delted Item");
            //currentObj = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            Block blockClass = eventData.pointerDrag.GetComponent<Block>();
            blockClass.DestroySelf();
            //Destroy(block);
            //Debug.LogError(blockClass.type.ToString());
        }
    }

    private Transform oldParent;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (clickable)
        {
            //Transform popupWindow = Instantiate(popupWindowPF);
            oldParent = popupWindow.parent;
            popupWindow.SetParent(transform);
            popupWindow.localPosition = new Vector3(-14, 20, -50);
            Text t = popupWindow.Find("Text").GetComponent<Text>();
            t.text = "Are you sure you would like to delete all the blocks?";
        }
    }

    public void ConfirmTrue()
    {
        Debug.Log("Delete - ConfirmTrue");
        resetQuestion();

        foreach (Transform child in codingArea)
        {
            if (child.tag != "CodingArea")
            {
                Block c = child.GetComponent<Block>();
                c.DestroySelf();
            }
        }


    }
    public void ConfrimFalse()
    {
        Debug.Log("Delete - ConfirmFalse");
        resetQuestion();
    }

    private void resetQuestion()
    {
        popupWindow.SetParent(oldParent);
        popupWindow.localPosition = new Vector3(-2000, 0, -100);
        clickable = true;
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
