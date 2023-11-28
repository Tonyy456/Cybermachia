using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlayers : MonoBehaviour
{
    public void DoEnablePlayers()
    {
        HordePlayer[] players = GameObject.FindObjectsOfType<HordePlayer>();
        foreach(var player in players)
        {
            player.EnableAllInputs(true);
        }
    }
}
