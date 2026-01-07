using System.Threading;
using UnityEngine;

[System.Serializable]
public class BerserkChargeInstance : AbilityInstance
{
    private float BaseTimer;
    private float BaseJumpForce;
    private float JumpForce;
    private float damage;
    public BerserkChargeInstance(BerserkChargeTemplate template) : base(template)
    {
        JumpForce = template.JumpForce;
        BaseJumpForce = template.JumpForce;
        BaseTimer = template.timer;
    }
    
    public override float CalculateScore(Unit unit, GameObject target)
    {
        float score = 0;
        if (IsOnCooldown()) return 0;

        float distance = Vector3.Distance(unit.transform.position, target.transform.position);
    
        if (distance < minDistanceToTarget) return 0;
        if (distance > maxDistanceToTarget) return 0;

        score = priority;
        score += (lastUsedTime * 0.25f);
        score += distance;
        
        if (distance < 5f)
            score += (5f - distance) * 2f;
            
        return score;
    }

    public override bool Execute(Unit inunit, GameObject target)
    {
        if (!base.Execute(inunit, target))
            return false;
        unit = inunit;
        abilityTarget = target;
        
        inunit.movementAuthority = MovementAuthority.Ability;
        inunit.Agent.isStopped = true;
        
        Rigidbody rb = inunit.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = false;
        
        Vector3 dir = target.transform.position - inunit.transform.position;
        rb.AddForce(dir * JumpForce);
        
        return true;
    }

    public override void Update(float dt)
    {
        base.Update(dt);

        if (Vector3.Distance(unit.transform.position, abilityTarget.transform.position) < 2)
        {
            //do damage
            OnAbilityEnd(unit);
        }
    }

    public override void OnAbilityEnd(Unit unit)
    {
        base.OnAbilityEnd(unit);
        unit.Agent.isStopped = false;
        Debug.Log("Berserk Charge ended");
    }

    public override void ResetAbility()
    {
        base.ResetAbility();
        usedTimer = timer;
    }
    
    public override void LevelUpAbility()
    {
        base.LevelUpAbility();

        damage += 5;
    }
}