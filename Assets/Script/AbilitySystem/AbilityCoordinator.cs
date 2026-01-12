using System.Collections.Generic;
using UnityEngine;

public class AbilityCoordinator : MonoBehaviour
{
    [SerializeField] private List<AbilityTemplate> abilityTemplates;
    [SerializeField] public List<AbilityInstance> abilities;

    private Unit unit;

    public Coroutine CurrentAbilityCoroutine;
    
    private bool AnAbilityIsCurrentlyRunning => CurrentAbility != null && CurrentAbility.isActive;
    public AbilityInstance CurrentAbility;

    private void Awake()
    {
        unit = GetComponent<Unit>();
        abilities = new List<AbilityInstance>();
    }

    public void LoadFromSave(List<AbilitySaveData> savedAbilities)
    {
        abilities = new List<AbilityInstance>();

        foreach (var save in savedAbilities)
        {
            var template = UnitFactory.Get(save.abilityId);
            var instance = template.CreateInstance();

            instance.level = save.level;

            abilities.Add(instance);
        }
    }

    public AbilityInstance RequestBestAbilityAvailable(AbilityType abilityType, Unit unit, GameObject target)
    {
        AbilityInstance bestAbility = null;
        float highestScore = float.MinValue;

        foreach (var ability in abilities)
        {
            if (ability.abilityType != abilityType)
                continue;

            float score = ability.CalculateScore(unit, target);
            
            if (score < 1f)
                continue;
            
            if (score > highestScore)
            {
                highestScore = score;
                bestAbility = ability;
            }
        }

        return bestAbility;
    }

    public bool FireAbility(AbilityInstance ability, Unit unit,  GameObject target)
    {
        if(CurrentAbility != null) return false;
        Debug.Log($"firing ability {ability.abilityType}");
        ability.Execute(unit, target);
        CurrentAbility = ability;
        ability.OnAbilityEnded += HandleAbilityEnded;
        return true;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        foreach (var ability in abilities)
            ability.TickCooldowns(dt);
    }

    public bool CanUseAbility()
    {
        return !AnAbilityIsCurrentlyRunning;
    }
    
    public void AddAbility(AbilityTemplate template)
    {
        abilities.Add(template.CreateInstance());
    }
    
    public void RemoveAbility(AbilityInstance ability)
    {
        if (CurrentAbility == ability)
            CurrentAbility = null;
            
        abilities.Remove(ability);
    }
    
    private void HandleAbilityEnded(AbilityInstance ability)
    {
        if (CurrentAbility == ability)
        {
            CurrentAbility = null;
        }
        ability.OnAbilityEnded -= HandleAbilityEnded;
        unit.GetRigidbody().linearVelocity = Vector3.zero;
        unit.movementAuthority = MovementAuthority.StateMachine;
    }
    
    public AbilityInstance GetAbilityByName(string name)
    {
        return abilities.Find(a => a.abilityName == name);
    }
}