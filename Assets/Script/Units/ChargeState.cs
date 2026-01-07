using UnityEngine;

public class ChargeState : State
{
    private ChargeStateSO config;
    private GameObject target;
    public ChargeState(Unit unit, StateMachine stateMachine, ChargeStateSO config) 
        : base(unit, stateMachine)
    {
        this.config = config;
    }
    
    public override void EnterState()
    {
        base.EnterState();
        AbilityInstance ability = unit.abilityCoordinator.RequestBestAbilityAvailable(AbilityType.OnCharge, unit, unit.targetGO);
        if (ability != null)
        {
            unit.abilityCoordinator.FireAbility(ability, unit, unit.targetGO);
        }
        else
        {
            Debug.Log("ability could not be found");
        }
        target = unit.targetGO;
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (target != null)
        {
            if (unit.movementAuthority == MovementAuthority.StateMachine)
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
                if (dis <= config.attackDistance)
                {
                    stateMachine.ChangeState(unit.AttackState);
                }
            }
        }
        else if (unit.Agent.hasPath)
        {
            unit.Agent.ResetPath(); 
        }
        else if (target == null)
        {
            unit.StateMachine.ChangeState(unit.ChaseState);
        }
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