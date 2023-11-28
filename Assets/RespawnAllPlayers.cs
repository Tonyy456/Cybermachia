using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnAllPlayers : MonoBehaviour
{
    public void RespawnPlayers()
    {
        HordePlayer[] players = GameObject.FindObjectsOfType<HordePlayer>();
        foreach(var player in players)
        {
            player.Respawn();
        }
    }
}
