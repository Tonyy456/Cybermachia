using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIL_GameStatistics : MonoBehaviour
{
    public List<AIL_PlayerStats> playerStats { get; private set; }

    const int numPlayers = 4;
    public void Awake()
    {
        playerStats = new List<AIL_PlayerStats>();
        for (int i = 0; i < numPlayers; i++) playerStats.Add(new AIL_PlayerStats());
    }

    public AIL_PlayerStats GetStatistics(PlayerInput input)
    {
        return playerStats[input.playerIndex];
    }

    public AIL_PlayerStats GetStatistics(int index)
    {
        return playerStats[index];
    }

    public int getWinner()
    {
        return playerStats.Max(x => x.PlayersKilled);
    }

    public void PlayerHitPlayer(PlayerInput shooter, PlayerInput shootee)
    {
        AIL_PlayerStats stat_shooter = playerStats[shooter.playerIndex];
        stat_shooter.BulletsHit += 1;

        //AIL_PlayerStats stat_shootee = playerStats[shootee.playerIndex];
    }

    public void PlayerKilled(PlayerInput shooter, PlayerInput shootee)
    {
        AIL_PlayerStats stat_shooter = playerStats[shooter.playerIndex];
        stat_shooter.PlayersKilled += 1;

        //AIL_PlayerStats stat_shootee = playerStats[shootee.playerIndex];
    }
}

public class AIL_PlayerStats
{
    public int BulletsHit { get; set; } = 0;
    public int PlayersKilled { get; set; } = 0;
    public AIL_PlayerStats()
    {

    }

    public void Reset()
    {
        this.BulletsHit = 0;
        this.PlayersKilled = 0;
    }
}
