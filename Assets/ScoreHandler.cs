using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    private const int maxPlayers = 4;
    // responsible ---> number of kills they got
    private List<int> playerKills = new List<int>();

    // playedKilled ---> (playerWhoGotTheKill --> num of times they killed the person)
    private List<Dictionary<int, int>> killedBy = new List<Dictionary<int, int>>();

    private int winningScore = 0;

    public void Awake()
    {
        playerKills.Clear();
        playerKills = new List<int>();
        for(int i = 0; i < maxPlayers; i++) playerKills.Add(0);

        killedBy.Clear();
        killedBy = new List<Dictionary<int, int>>();
        for (int i = 0; i < maxPlayers; i++) killedBy.Add(
            new Dictionary<int, int>());
    }

    public void PlayerKilled(int deadPlayerIndex, int responsiblePlayerIndex)
    {
        playerKills[responsiblePlayerIndex] += 1;
        winningScore = Mathf.Max(winningScore, playerKills[responsiblePlayerIndex]);

        if (killedBy[deadPlayerIndex].ContainsKey(responsiblePlayerIndex))
            killedBy[deadPlayerIndex][responsiblePlayerIndex] += 1;
        else
            killedBy[deadPlayerIndex][responsiblePlayerIndex] = 0;
    }

    public int NumberOfKills(int playerIndex)
    {
        return playerKills[playerIndex];
    }

    public int WinningPlayer(out List<int> players)
    {
        players = new List<int>();
        for(int i = 0; i < playerKills.Count; i++)
        {
            if (playerKills[i] == winningScore)
            {
                players.Add(i);
            }
        }
        return winningScore;
    }
}
