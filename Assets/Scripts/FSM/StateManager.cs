using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager
{
    private IState currentState;
    public IState CurrentState => currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Execute();
    }
}
