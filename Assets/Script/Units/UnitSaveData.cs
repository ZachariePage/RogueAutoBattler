using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitSaveData
{
    public UnitDefinition baseUnit;
    public string unitId;
    public float health;
    public float damage;

    public void BuffHealth(float health)
    {
        this.health += health;
    }

    public void BuffDamage(float damage)
    {
        this.damage += damage;
    }

    public List<AbilitySaveData> abilities;
}
