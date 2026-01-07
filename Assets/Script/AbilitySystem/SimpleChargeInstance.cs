using UnityEngine;

[System.Serializable]
public class SimpleChargeInstance : AbilityInstance
{
    private float damage;
    public SimpleChargeInstance(SimpleChargeTemplate template) : base(template)
    {
    }
    
    public override float CalculateScore(Unit unit, GameObject target)
    {
        if (IsOnCooldown()) 
            return 0;

        float score = priority;
        score += (lastUsedTime * 0.25f);
        
        return score;
    }

    public override bool Execute(Unit unit, GameObject target)
    {
        if (!base.Execute(unit, target))
            return false;
        
        Debug.Log($"Executing SimpleCharge on {target.name}");
        
        return true;
    }

    public override void OnAbilityEnd(Unit unit)
    {
        base.OnAbilityEnd(unit);
        Debug.Log("SimpleCharge ended");
    }

    public override void LevelUpAbility()
    {
        base.LevelUpAbility();

        damage += 5;
    }
}