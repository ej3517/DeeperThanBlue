using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour
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

        if (clickable)
        {
            codingArea.ButtonStart();
            clickable = false;
        }
    }

    public void Restart()
    {
        codingArea.Restart();
    }

    public void Forward()
    {
        codingArea.ControlCommand(CodingArea.BlockCommand.Forward);
    }

    public void TurnLeft()
    {
        codingArea.ControlCommand(CodingArea.BlockCommand.TurnLeft);
    }

    public void TurnRight()
    {
        codingArea.ControlCommand(CodingArea.BlockCommand.TurnRight);
    }
}
