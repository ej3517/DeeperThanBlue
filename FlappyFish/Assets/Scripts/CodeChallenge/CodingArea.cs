﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodingArea : MonoBehaviour
{
    public Transform StartBlock;
    public Transform Button;
    public Transform popupWindowError;

    public Transform gameAreaGrid;
    private GameAreaGrid grid;

    public event EventHandler OnButtonStart;
    public event EventHandler ResetEvent;
    public event EventHandler<CodingArgs> ActionEvent;

    private CodingArea instance;
    private StartButton startButton;

    public enum BlockCommand
    {
        Forward,
        TurnLeft,
        TurnRight,
    };

    public class CodingArgs : EventArgs
    {
        public BlockCommand instructionType;
    }

    public void Awake()
    {
        startButton = Button.GetComponent<StartButton>();
        grid = gameAreaGrid.GetComponent<GameAreaGrid>();
    }

    CodingArea()
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
        startButton.setClick(true);
        // TODO: Event restart to grid
        ResetEvent?.Invoke(this, EventArgs.Empty);
    }

    Vector3 oldWindowPosition;
    public void ButtonStart()
    {
        if (!StartBlock.GetComponent<Block>().Validate())
        {
            Debug.LogError("Invalid Structure");
            oldWindowPosition = popupWindowError.position;
            popupWindowError.position = new Vector3(879, -5, -570);
            Text t = popupWindowError.Find("Text").GetComponent<Text>();
            t.text = "Invalid Structure";
            // TODO: ADD debug string to validate function
            return;
        }

        //Move start to first object
        Block _start = StartBlock.GetComponent<Block>();
        StartCoroutine(_start.Traverse(Button));

        OnButtonStart?.Invoke(this, EventArgs.Empty);
    }

    public void CloseWindow()
    {
        Debug.Log("Closed popup-window");
        popupWindowError.position = oldWindowPosition;
        Restart();
    }

    public void ControlCommand(BlockCommand _instructionType)       //Add type as arg
    {
        ActionEvent?.Invoke(this, new CodingArgs { instructionType = _instructionType });
    }

}
