using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance { get; private set; }

    public GameObject[] spawnzones;
    public List<AbilityTemplate> abilities;

    public static Dictionary<string, AbilityTemplate> lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate UnitFactory detected, destroying.");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        InitializeLookup();
    }

    private void Update()
    {
       
    }

    public void SpawnUnit(UnitSaveData data, UnitDefinition def, Vector3 pos, string team)
    {
        GameObject go = Instantiate(def.prefab, pos, Quaternion.identity);
        Unit unit = go.GetComponent<Unit>();
        
        unit.health = data.health;
        unit.damage = data.damage;

        unit.abilityCoordinator.LoadFromSave(data.abilities);
    }

    private void InitializeLookup()
    {
        lookup = new Dictionary<string, AbilityTemplate>();

        foreach (var ability in abilities)
        {
            if (ability == null)
                continue;

            if (string.IsNullOrEmpty(ability.abilityId))
            {
                Debug.LogError($"Ability {ability.abilityName} has no ID");
                continue;
            }

            if (lookup.ContainsKey(ability.abilityId))
            {
                Debug.LogError($"Duplicate ability ID: {ability.abilityId}");
                continue;
            }

            lookup.Add(ability.abilityId, ability);
        }
    }

    public static AbilityTemplate Get(string id)
    {
        if (lookup == null)
        {
            Debug.LogError("UnitFactory not initialized.");
            return null;
        }

        if (!lookup.TryGetValue(id, out var ability))
        {
            Debug.LogError($"Ability ID not found: {id}");
        }

        return ability;
    }
}