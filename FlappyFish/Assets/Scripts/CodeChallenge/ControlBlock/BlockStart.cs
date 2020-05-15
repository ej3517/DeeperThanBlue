using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockStart : Block
{

    private void Awake()
    {
        type = "Start";
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        ;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        ;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        ;
    }
}
