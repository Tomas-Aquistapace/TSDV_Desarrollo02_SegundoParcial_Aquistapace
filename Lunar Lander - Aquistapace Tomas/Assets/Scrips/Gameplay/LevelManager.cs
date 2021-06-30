using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int actualLevel;

    public List<GameObject> terrainsPref;
    public GameObject FinalScreen;

    GameObject actualTerrain;

    void Start()
    {
        instance = this;

        actualLevel = 1;

        SelectLevel();

        Player_Ship.WinLevel += ChangeLevel;
        Player_Ship.FinishMission += CallFinalScreen;
    }

    // ----------------------

    void SelectLevel()
    {
        int rand = Random.Range(1, terrainsPref.Count);

        actualTerrain = Instantiate(terrainsPref[rand]);
        actualTerrain.transform.name = terrainsPref[rand].transform.name;
    }

    void ChangeLevel(bool value)
    {
        Destroy(actualTerrain);
        SelectLevel();

        if(value)
            actualLevel++;
    }
    
    void CallFinalScreen()
    {
        FinalScreen.SetActive(true);
    }
}
