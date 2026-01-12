using UnityEngine;

[CreateAssetMenu(menuName = "Autobattler/Army Generation Settings")]
public class ArmyGenerationSettings : ScriptableObject
{
    [Header("Budget Split")]
    [Range(0.6f, 0.8f)]
    public float unitBudgetPercent = 0.7f;

    [Range(0f, 1f)]
    public float statBuffPercentOfRemaining = 0.5f;

    [Header("Army Size Limits")]
    public int minUnits = 2;
    public int maxUnits = 8;

    [Header("Stat Buffing")]
    public float healthBuffStep = 10f;
    public float damageBuffStep = 2f;
    public int healthBuffCost = 5;
    public int damageBuffCost = 5;

    [Header("Abilities")]
    public int maxAbilitiesPerUnit = 10;
    public float chanceToBuyNewAbility = 0.6f;
    public int upgradeCost = 5;

    [Header("Randomness")]
    [Range(0f, 1f)]
    public float favorCheapUnits = 0.3f; // 0 = totally random, 1 = strongly favors cheap units
}