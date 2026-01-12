using System;
using UnityEngine;

[Serializable]
public class AbilitySaveData
{
    public string abilityId;
    public int level;
    public float cooldownModifier;
    
    public AbilitySaveData(string id, int level, float cooldownModifier)
    {
        abilityId = id;
        this.level = level;
        this.cooldownModifier = cooldownModifier;
    }
    
    //copy constructor
    public AbilitySaveData(AbilitySaveData other)
    {
        abilityId = other.abilityId;
        level = other.level;
        cooldownModifier = other.cooldownModifier;
    }
}
