using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    public GameObject[] spawnzonesEnemy;
    public GameObject[] spawnzonesPlayer;

    public List<UnitSaveData> enemyUnit;
    public List<UnitSaveData> playerUnits;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyUnit = new List<UnitSaveData>();
        playerUnits = new List<UnitSaveData>();

        foreach (UnitSaveData unit in AiArmy.Instance.units)
        {
            enemyUnit.Add(new UnitSaveData(unit));
        }
        
        foreach (UnitSaveData unit in CampaignRoster.Instance.units)
        {
            playerUnits.Add(new UnitSaveData(unit));
        }
        
        SpawnArmies(enemyUnit, "Team2", "Team1",spawnzonesEnemy);
        SpawnArmies(playerUnits, "Team1", "Team2",spawnzonesPlayer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnArmies(List<UnitSaveData> Units, string team, string enemyTeam, GameObject[] spawnzone)
    {
        for (int i = 0; i < Units.Count; i++)
        {
            UnitFactory.Instance.SpawnUnit(Units[i], Units[i].baseUnit, spawnzone[i].transform.position, team, enemyTeam);
        }
    }
}
