using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    private Transform currentObj;
    public void OnDrop(PointerEventData eventData)
    {
        Debug.LogError("Dropped in box");
        if (eventData.pointerDrag != null)
        {
            currentObj = GetComponent<Transform>();
            Transform block = eventData.pointerDrag.GetComponent<Transform>();
            Block blockClass = eventData.pointerDrag.GetComponent<Block>();
            block.transform.SetParent(currentObj);
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = /*GetComponent<RectTransform>().anchoredPosition - */ new Vector3(0, -70, 0); //TODO make size dynamic
            //Debug.LogError(blockClass.type.ToString());
        }


    }
}


/*
 You can try this: objectB.position = parentTransform.InverseTransformPoint(objectC.position);

That will convert the objectC.position into the local space for the parentTransform, and then assign it to objectB
*/
