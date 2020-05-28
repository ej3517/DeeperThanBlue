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

    public override bool Validate()
    {
        if (belowBlock == null)
        {
            return false;
        }
        else
        {
            return belowBlock.Validate();
        }
    }

    public override IEnumerator Traverse(Transform Button)
    {
        yield return new WaitForSeconds(0);
        if (belowBlock!=null)
        {
            StartCoroutine(belowBlock.Traverse(Button));
        }
    }

    public override void DestroySelf()
    {
        if (belowBlock != null)
        {
            belowBlock.DestroySelf();
        }
        //Destroy(transform.gameObject);    //Don't destroy start
    }



    //OVERRIDES 
    public override void OnBeginDrag(PointerEventData eventData) {;}
    public override void OnDrag(PointerEventData eventData) {;}
    public override void OnEndDrag(PointerEventData eventData) {;}

}
