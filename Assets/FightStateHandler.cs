using System.Collections;
using System.Collections.Generic;
using Tony;
using UnityEngine;

public class FightStateHandler : MonoBehaviour
{
    [SerializeField] private StateSO fightState;
    public void Start()
    {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        foreach(var player in players)
        {
            player.EnablePlayer();
        }
    }
}
