using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TIL_GameStatistics : MonoBehaviour
{
    public List<TIL_PlayerStats> playerStats { get; private set; }

    const int numPlayers = 4;
    public void Awake()
    {
        playerStats = new List<TIL_PlayerStats>();
        for (int i = 0; i < numPlayers; i++) playerStats.Add(new TIL_PlayerStats());
    }

    public TIL_PlayerStats GetStatistics(PlayerInput input)
    {
        return playerStats[input.playerIndex];
    }

    public TIL_PlayerStats GetStatistics(int index)
    {
        return playerStats[index];
    }

    public void PlayerHitPlayer(PlayerInput shooter, PlayerInput shootee)
    {
        TIL_PlayerStats stat_shooter = playerStats[shooter.playerIndex];
        stat_shooter.BulletsHit += 1;

        //TIL_PlayerStats stat_shootee = playerStats[shootee.playerIndex];
    }

    public void PlayerKilled(PlayerInput shooter, PlayerInput shootee)
    {
        TIL_PlayerStats stat_shooter = playerStats[shooter.playerIndex];
        stat_shooter.PlayersKilled += 1;

        //TIL_PlayerStats stat_shootee = playerStats[shootee.playerIndex];
    }
}

public class TIL_PlayerStats
{
    public int BulletsHit { get; set; } = 0;
    public int PlayersKilled { get; set; } = 0;
    public TIL_PlayerStats()
    {

    }

    public void Reset()
    {
        this.BulletsHit = 0;
        this.PlayersKilled = 0;
    }
}
