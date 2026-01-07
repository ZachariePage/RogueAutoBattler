using System.Collections.Generic;
using UnityEngine;

public class UnitBank : MonoBehaviour
{
    public List<UnitDefinition> units;
    public List<AbilityTemplate> abilities;

    public ArmyGenerationSettings settings;
    
    public static UnitBank Instance;
    
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
}
