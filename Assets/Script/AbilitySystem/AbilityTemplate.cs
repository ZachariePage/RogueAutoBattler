using System;
using System.Collections.Generic;
using UnityEngine;
public enum AbilityType
{
    OnTargetAcquired,
    OnCharge,
    OnAttack,
    OnAbilityWindow,
    OnDamaged,
    OnDeath
}

[Serializable]
public abstract class AbilityTemplate : ScriptableObject
{
    [Header("Base Settings")]
    public string abilityName;
    public string abilityId;
    public AbilityType abilityType;
    
    [Header("Ability Timers")]
    public float timer;
    
    [Header("Stats")]
    public float baseCooldown = 10f;
    public float basePriority = 1f;
    
    [Header("Conditions")]
    public float minDistanceToTarget = 0f;
    public float maxDistanceToTarget = Mathf.Infinity;
    
    [Header("cost")]
    public int cost;
    
    public abstract AbilityInstance CreateInstance();
}