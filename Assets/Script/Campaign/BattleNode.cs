using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class BattleNode : Nodes
{
    private int UnitsPower;

    [SerializeField] private List<UnitSaveData> Units;
    
    public BattleNode(int id, int depth, int path, int unitsPower) : base(id, depth, path)
    {
        UnitsPower = unitsPower;
        type = NodeType.Fight;
        Units = new List<UnitSaveData>();
        CreateUnitStack();
    }

    public override void Execute()
    {
        base.Execute();
        SceneLoader.Instance.LoadFightScene(Units);
    }

    private void CreateUnitStack()
    {
        int money = UnitsPower;
        Units = new List<UnitSaveData>();
        UnitBank bank = UnitBank.Instance;
        ArmyGenerationSettings settings = bank.settings;

        int unitBudget = Mathf.RoundToInt(money * settings.unitBudgetPercent);
        int remainingBudget = money - unitBudget;

        int statBudget = Mathf.RoundToInt(remainingBudget * settings.statBuffPercentOfRemaining);
        int abilityBudget = remainingBudget - statBudget;
        
        List<UnitDefinition> availableUnits = new List<UnitDefinition>(bank.units);
        
        int safety = 1000;
        while (!BudgetIsLessThanLeastExpensiveUnit(unitBudget) && safety-- > 0)
        {
            UnitDefinition randomUnit = UnitBank.Instance.units[Random.Range(0, UnitBank.Instance.units.Count)];

            int cost = randomUnit.BaseCost();
            
            if (cost > unitBudget) continue;

            unitBudget -= cost;
            List<AbilitySaveData> startingabilities = new List<AbilitySaveData>();
            foreach (AbilityTemplate ability in randomUnit.startingAbilities)
            {
                startingabilities.Add(new AbilitySaveData(ability.abilityId, 0, 0f));
            }
            
            Units.Add(new UnitSaveData(randomUnit, randomUnit.ToString(), randomUnit.baseHealth, randomUnit.baseDamage, startingabilities));
        }
        

        if (Units.Count == 0)
            return;
        
        safety = 1000;

        while (!BudgetIsLessThanCheapestStatBuff(statBudget, settings) && safety-- > 0)
        {
            UnitSaveData unit = Units[Random.Range(0, Units.Count)];

            bool buffHealth = Random.value > 0.5f;

            if (buffHealth && statBudget >= settings.healthBuffCost)
            {
                unit.BuffHealth(settings.healthBuffStep);
                statBudget -= settings.healthBuffCost;
            }
            else if (!buffHealth && statBudget >= settings.damageBuffCost)
            {
                unit.BuffDamage(settings.damageBuffStep);
                statBudget -= settings.damageBuffCost;
            }
        }

        safety = 1000;

        while (!BudgetIsLessThanCheapestAbilityAction(abilityBudget, settings) && safety-- > 0)
        {
            bool tryUpgrade = Random.value < 0.5f;

            //upgrade
            if (tryUpgrade && abilityBudget >= settings.upgradeCost)
            {
                List<UnitSaveData> upgradableUnits = Units.FindAll(u => u.abilities.Count > 0);

                if (upgradableUnits.Count > 0)
                {
                    UnitSaveData unit = upgradableUnits[Random.Range(0, upgradableUnits.Count)];
                    AbilitySaveData ability =
                        unit.abilities[Random.Range(0, unit.abilities.Count)];

                    ability.level++;
                    abilityBudget -= settings.upgradeCost;
                    continue;
                }
            }

            //buy
            List<UnitSaveData> validUnits = Units.FindAll(u => u.abilities.Count < settings.maxAbilitiesPerUnit);

            if (validUnits.Count == 0) continue;

            AbilityTemplate template = UnitBank.Instance.abilities[Random.Range(0, UnitBank.Instance.abilities.Count)];

            if (template.cost > abilityBudget) continue;

            UnitSaveData target = validUnits[Random.Range(0, validUnits.Count)];
            
            AbilitySaveData existing = target.abilities.Find(a => a.abilityId == template.abilityId);

            if (existing != null)
            {
                if (abilityBudget >= settings.upgradeCost)
                {
                    existing.level++;
                    abilityBudget -= settings.upgradeCost;
                }
                continue;
            }
            
            abilityBudget -= template.cost;
            target.abilities.Add(new AbilitySaveData(template.abilityId, 0, 0f));
        }

    }
    
    private UnitDefinition ChooseUnitWeighted(List<UnitDefinition> units, float cheapBias)
    {
        if (cheapBias <= 0f)
            return units[Random.Range(0, units.Count)];

        float totalWeight = 0f;
        foreach (var u in units)
            totalWeight += 1f / u.BaseCost();

        float roll = Random.value * totalWeight;

        foreach (var u in units)
        {
            roll -= 1f / u.BaseCost();
            if (roll <= 0)
                return u;
        }

        return units[0];
    }

    public bool BudgetIsLessThanLeastExpensiveUnit(int budget)
    {
        UnitBank bank = UnitBank.Instance;
        int lowestCost = int.MaxValue;

        foreach (UnitDefinition unit in bank.units)
        {
            lowestCost = Mathf.Min(unit.BaseCost(), lowestCost);
        }

        return budget < lowestCost;
    }
    
    public bool BudgetIsLessThanCheapestStatBuff(int budget, ArmyGenerationSettings settings)
    {
        int cheapestBuff = int.MaxValue;

        if (settings.healthBuffCost > 0)
            cheapestBuff = Mathf.Min(cheapestBuff, settings.healthBuffCost);

        if (settings.damageBuffCost > 0)
            cheapestBuff = Mathf.Min(cheapestBuff, settings.damageBuffCost);
        
        if (cheapestBuff == int.MaxValue)
            return true;

        return budget < cheapestBuff;
    }
    
    public bool BudgetIsLessThanCheapestAbilityAction(int budget, ArmyGenerationSettings settings)
    {
        UnitBank bank = UnitBank.Instance;
        int cheapest = int.MaxValue;
        
        foreach (AbilityTemplate ability in bank.abilities)
        {
            if (ability.cost > 0)
                cheapest = Mathf.Min(cheapest, ability.cost);
        }
        
        if (settings.upgradeCost > 0)
            cheapest = Mathf.Min(cheapest, settings.upgradeCost);
        
        if (cheapest == int.MaxValue)
            return true;

        return budget < cheapest;
    }




}
