using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int actualLevel;
    public static LevelManager instance;

    void Start()
    {
        instance = this;

        actualLevel = 1;

        Player_Ship.WinLevel += ChangeLevel;
    }

    // ----------------------

    public void ChangeLevel()
    {
        actualLevel++;
    }
    

}
