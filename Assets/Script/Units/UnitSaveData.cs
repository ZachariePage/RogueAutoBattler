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
    
    
    //copy construtor
    public UnitSaveData(UnitSaveData other)
    {
        baseUnit = other.baseUnit;
        unitId = other.unitId;
        health = other.health;
        damage = other.damage;

        abilities = new List<AbilitySaveData>();
        foreach (var ability in other.abilities)
        {
            abilities.Add(new AbilitySaveData(ability));
        }
    }

    public UnitSaveData(UnitDefinition baseUnit, string unitId, float health, float damage, List<AbilitySaveData> abilities)
    {
        this.baseUnit = baseUnit;
        this.unitId = unitId;
        this.health = health;
        this.damage = damage;
        this.abilities = abilities;
    }

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
