using UnityEngine;

public class State
{
    protected Unit unit;
    protected StateMachine stateMachine;

    public State(Unit unit, StateMachine stateMachine)
    {
        this.unit = unit;
        this.stateMachine = stateMachine;
    }

    public virtual void EnterState()
    {
        StateScriptableObject config = stateMachine.CurrentState.GetConfig();
    }

    public virtual void ExitState()
    {
    }

    public virtual void FrameUpdate()
    {

    }
    public virtual void PhysicUpdate()
    {
    
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        
    }
    public virtual StateScriptableObject GetConfig()
    {
        return null;
    }
}
