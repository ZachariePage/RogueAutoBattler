using UnityEngine;

public class AttackState : State
{
    private AttackStateSO config;
    
    public AttackState(Unit unit, StateMachine stateMachine, AttackStateSO config) : base(unit, stateMachine)
    {
        this.config = config;
    }
    
    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }
    
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }
    public override StateScriptableObject GetConfig()
    {
        return config;
    }
}
