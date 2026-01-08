using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    
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

    public void LoadFightScene(List<UnitSaveData> Units)
    {
        AiArmy.Instance.SetUnits(Units);
        SceneManager.LoadScene("FightScene");
    }
}
