using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance { get; private set; }

    public static Dictionary<string, AbilityTemplate> lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeLookup();
    }

    private void Update()
    {
       
    }

    public void SpawnUnit(UnitSaveData data, UnitDefinition def, Vector3 pos, string team, string enemyTeam)
    {
        GameObject newUnit = Instantiate(def.prefab, pos, Quaternion.identity);
        Unit unit = newUnit.GetComponent<Unit>();
        
        unit.health = data.health;
        unit.damage = data.damage;
        unit.MyUnit = def;

        unit.abilityCoordinator.LoadFromSave(data.abilities);

        newUnit.tag = team;
        newUnit.layer = LayerMask.NameToLayer(team);
        unit.teamString = enemyTeam;
        
        unit.Initialize();
    }

    private void InitializeLookup()
    {
        lookup = new Dictionary<string, AbilityTemplate>();

        foreach (var ability in UnitBank.Instance.abilities)
        {
            if (ability == null)
                continue;

            if (string.IsNullOrEmpty(ability.abilityId))
            {
                continue;
            }

            if (lookup.ContainsKey(ability.abilityId))
            {
                continue;
            }

            lookup.Add(ability.abilityId, ability);
        }
    }

    public static AbilityTemplate Get(string id)
    {
        if (lookup == null)
        {
            return null;
        }

        if (!lookup.TryGetValue(id, out var ability))
        {
        }

        return ability;
    }
}