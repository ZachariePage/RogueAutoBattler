using System.Collections.Generic;
using UnityEngine;

public class CampaignRoster : MonoBehaviour
{
    public List<UnitSaveData> units;

    public static CampaignRoster Instance;

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

    public void AddUnit(UnitDefinition def)
    {
        
    }
    
}