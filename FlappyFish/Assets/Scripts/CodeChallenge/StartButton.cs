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

    public void AddReturn(Transform t)
    {
        codingArea.AddReturn(t);
    }
    public void End()
    {
        try
        {
            Block block = codingArea.PopReturn().GetComponent<Block>();
            StartCoroutine(block.Traverse(transform));
        }
        catch
        {
            //System.Threading.Thread.Sleep(1000);
            Restart();
        }
    }

    public bool Forward()
    {
        return codingArea.ControlCommand(CodingArea.BlockCommand.Forward);
    }

    public void TurnLeft()
    {
        codingArea.ControlCommand(CodingArea.BlockCommand.TurnLeft);
    }

    public void TurnRight()
    {
        codingArea.ControlCommand(CodingArea.BlockCommand.TurnRight);
    }

    public int GetVar(string s)
    {
        return codingArea.GetVar(s);
    }
    public void SetVar(string s, int val)
    {
        codingArea.SetVar(s, val);
    }
}
