using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodingArea : MonoBehaviour
{
    public Transform Start;
    public Transform Button;

    public event EventHandler OnButtonStart;

    CodingArea instance;
    private void Awake()
    {
        instance = this;
    }

    public CodingArea GetInstance()
    {
        return instance;
    }

    public void Restart()
    {
        Button.SetParent(transform);
        Button.localPosition = new Vector3(-200, 290, -1);

        // TODO: Event restart to grid
    }

    public void ButtonStart()
    {
        if(!Start.GetComponent<Block>().Validate())
        {
            Debug.LogError("Invalid Structure");
            return;
        }

        //Move start to first object
        Block _start = Start.GetComponent<Block>();
        StartCoroutine(_start.Traverse(Button));

        OnButtonStart?.Invoke(this, EventArgs.Empty);
    }

    public void ControlCommand()       //Add type as arg
    {
        ;
    }
}
