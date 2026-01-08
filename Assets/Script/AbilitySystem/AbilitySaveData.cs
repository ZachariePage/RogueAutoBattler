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
}
