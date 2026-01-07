using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Unit/A unit")]
public class UnitDefinition : ScriptableObject
{
    public GameObject prefab;
    public int UnitCost = 10;
    public float baseHealth;
    public float baseDamage;
    public List<AbilityTemplate> startingAbilities;

    public int BaseCost()
    {
        int cost = 0;
        foreach (AbilityTemplate ability in startingAbilities)
        {
            cost += ability.cost;
        }
        return UnitCost + cost;
    }
}
