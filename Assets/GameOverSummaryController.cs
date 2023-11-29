using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverSummaryController : MonoBehaviour
{
    [SerializeField] private GameObject turnOnGameOver;
    [SerializeField] private TMPro.TMP_Text roundResultText;
    public void GameOver()
    {
        turnOnGameOver.SetActive(true);
        HordeSpawner spawner = GameObject.FindObjectOfType<HordeSpawner>();
        if (spawner) roundResultText.text = ""+spawner.Round;
    }

}
