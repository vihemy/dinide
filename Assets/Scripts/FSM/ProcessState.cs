using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingState : IState
{
    public void Enter() { /* Logic to execute when entering the idle state */ }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit() { /* Logic to execute when exiting the idle state */ }
}