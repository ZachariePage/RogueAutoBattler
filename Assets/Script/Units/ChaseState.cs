using UnityEngine;
using UnityEngine.AI;

public class ChaseState : State
{
    private ChaseStateSO config;
    private GameObject target;
    
    public ChaseState(Unit unit, StateMachine stateMachine, ChaseStateSO config) 
        : base(unit, stateMachine)
    {
        this.config = config;
    }
    
    public override void EnterState()
    {
        base.EnterState();
        unit.movementAuthority = MovementAuthority.StateMachine;
        target = unit.FindClosestPlayerInRadius(unit.transform.position, config.LookForRadius);
        unit.Agent.stoppingDistance = config.AgentStopDistance;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (unit.movementAuthority == MovementAuthority.Ability)
            return;
        
        if (target != null)
        {
            unit.Agent.SetDestination(target.transform.position);

            Vector3 directionToTarget = (target.transform.position - unit.transform.position).normalized;
            directionToTarget.y = 0;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                unit.transform.rotation = Quaternion.Slerp(unit.transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            
            float dis = Vector3.Distance(unit.transform.position, target.transform.position);
            if (dis <= config.chargeDistance)
            {
                if (dis <= config.attackDistance)
                {
                    
                }
                else
                {
                    stateMachine.ChangeState(unit.ChargeState);
                }
            }
        }
        else if (unit.Agent.hasPath)
        {
            unit.Agent.ResetPath(); 
        }
        else if (target == null)
        {
            target = unit.FindClosestPlayerInRadius(unit.transform.position, config.LookForRadius);
        }
    }
    
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override StateScriptableObject GetConfig()
    {
        return config;
    }
}