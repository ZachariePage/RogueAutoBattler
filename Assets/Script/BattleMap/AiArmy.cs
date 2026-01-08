using System.Collections.Generic;
using UnityEngine;

public class AiArmy : MonoBehaviour
{
    public List<UnitSaveData> units;

    public static AiArmy Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void SetUnits(List<UnitSaveData> newUnits)
    {
        if (Instance == null)
        {
            GameObject go = new GameObject("AiArmy");
            Instance = go.AddComponent<AiArmy>();
            DontDestroyOnLoad(go);
        }
        
        units.Clear();
        
        foreach (UnitSaveData unit in newUnits)
        {
            units.Add(new UnitSaveData
            {
                baseUnit = unit.baseUnit,
                unitId = unit.unitId,
                health = unit.health,
                damage = unit.damage,
                abilities = new List<AbilitySaveData>(unit.abilities)
            });
        }
    }
}
