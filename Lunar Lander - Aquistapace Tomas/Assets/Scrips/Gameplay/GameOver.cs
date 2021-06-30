using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private TextMeshProUGUI finishLevel;
    [SerializeField] private TextMeshProUGUI finishPoints;

    private void OnEnable()
    {
        finishLevel.text = LevelManager.instance.actualLevel.ToString();

        finishPoints.text = player.GetComponent<Player_Ship>().points.ToString();
    }
}
