using System;
using UnityEngine;

[System.Serializable]
public abstract class AbilityInstance
{
    public AbilityTemplate template;
    public AbilityType abilityType;
    public string abilityName;
    public float level;
    
    public float cooldown;
    public float priority;
    public float minDistanceToTarget;
    public float maxDistanceToTarget;
    
    public float currentCooldown = 0f;
    public float lastUsedTime = 0f;
    public bool isActive = false;
    
    protected float timer = 5f;
    protected float usedTimer;

    public Unit unit;
    public GameObject abilityTarget;
    
    public Action<AbilityInstance> OnAbilityEnded;
    
    public AbilityInstance(AbilityTemplate template)
    {
        this.template = template;
        this.abilityType = template.abilityType;
        this.abilityName = template.abilityName;
        
        cooldown = template.baseCooldown;
        priority = template.basePriority;
        minDistanceToTarget = template.minDistanceToTarget;
        maxDistanceToTarget = template.maxDistanceToTarget;

        this.timer = template.timer;
        usedTimer = timer;
    }
    
    public bool IsOnCooldown()
    {
        return currentCooldown > 0f;
    }
    
    public void ResetCooldown()
    {
        currentCooldown = cooldown;
        lastUsedTime = 0f;
    }

    public virtual void ResetAbility()
    {
        ResetCooldown();
    }
    
    public virtual void Update(float dt)
    {
        usedTimer -= dt;
        if (usedTimer <= 0f)
        {
            OnAbilityEnd(unit);
        }
    }

    public virtual void TickCooldowns(float dt)
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= dt;
        }
        lastUsedTime += dt;
    }
    
    public virtual float CalculateScore(Unit unit, GameObject target)
    {
        if (IsOnCooldown())
            return 0f;
        
        return priority;
    }
    
    public virtual bool CanUse(Unit unit, GameObject target)
    {
        return CalculateScore(unit, target) > 0f;
    }
    
    public virtual bool Execute(Unit unit, GameObject target)
    {
        if (!CanUse(unit, target))
            return false;
            
        isActive = true;
        return true;
    }
    
    public virtual void OnAbilityEnd(Unit unit)
    {
        ResetAbility();
        isActive = false;
        unit = null;
        abilityTarget = null;
        OnAbilityEnded?.Invoke(this);
    }

    public virtual void LevelUpAbility()
    {
        
    }
}