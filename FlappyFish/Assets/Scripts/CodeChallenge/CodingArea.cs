using System;
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
    //public event EventHandler<CodingArgs> ActionEvent;

    private CodingArea instance;
    private StartButton startButton;

    private Dictionary<string, int> variables;

    private Stack<Transform> scopeReturns;

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

        scopeReturns = new Stack<Transform>();

        CreateVarDict();
    }
    private void CreateVarDict()
    {
        variables = new Dictionary<string, int>();
        variables.Add("X", 0);
        variables.Add("Y", 0);
        variables.Add("Z", 0);
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
        CreateVarDict();
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

    public bool ControlCommand(BlockCommand _instructionType)       //Add type as arg
    {
        bool temp = grid.CodingAreaInstruction(_instructionType);
        if(!temp)
        {
            Restart();
        }
        return temp;
    }

    public int GetVar(string var)
    {
        if (variables.ContainsKey(var))
        {
            return variables[var];
        }
        Debug.LogError($"Invalid variable lookup. Looked up variable {var}, which does not exist in the variable map.");
        throw new System.InvalidOperationException("Invalid variable lookup");
    }
    public void SetVar(string var, int val)
    {
        if (variables.ContainsKey(var))
        {
            // TODO: Check if value is larger than 99?
            if (val > 99)
            {
                Debug.Log($"Limiting {val} to 99");
                val = 99;
            }
            if (val < 0)
            {
                Debug.Log($"Limiting {val} to 0");
                val = 0;
            }
            variables[var] = val;
        }
        else
        {
            Debug.LogError($"Invalid variable lookup. Looked up variable {var}, which does not exist in the variable map.");
            throw new System.InvalidOperationException("Invalid variable lookup");
        }
    }

    public void AddReturn(Transform t)
    {
        scopeReturns.Push(t);
    }

    public Transform PopReturn()
    {
        if (scopeReturns.Count > 0)
            return scopeReturns.Pop();
        else
            throw new Exception();
    }
}
