using UnityEngine;


public class StateMachine
{
     public State CurrentState;
    
     public void Init(State startingState)
     {
         CurrentState = startingState;
         DebugPrintStateName("entering");
         startingState.EnterState();
     }
    
     public void ChangeState(State newState)
     {
         DebugPrintStateName("exiting");
         CurrentState.ExitState();
         CurrentState = newState;
         CurrentState.EnterState();
         DebugPrintStateName("entering");
     }
    
     private void DebugPrintStateName(string message)
     {
         Debug.Log($"{message} {CurrentState}");
     }
}
