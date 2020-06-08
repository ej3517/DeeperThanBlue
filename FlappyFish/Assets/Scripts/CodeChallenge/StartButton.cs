using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton: MonoBehaviour
{
    CodingArea codingArea;
    bool clickable;
    public void setClick(bool _in)
    {
        clickable = _in;
    }


    public void Awake()
    {
        Transform ca = transform.parent;
        codingArea = ca.GetComponent<CodingArea>().GetInstance();
        clickable = true;
    }

    public void OnMouseDown()
    {

        if(clickable)
        {
            codingArea.ButtonStart();
            clickable = false;
        }
    }

    public void Restart()
    {
        codingArea.Restart();
    }
}
