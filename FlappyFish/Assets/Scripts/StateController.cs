using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum State
    {
        WaitingToStart,
        Playing,
        WaitingAnswer,
        Dead,
        Won
    }

    public State currentState;

    private void Awake()
    {
        currentState = State.WaitingToStart;
    }
}
