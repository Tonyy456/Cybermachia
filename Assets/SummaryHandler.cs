using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class SummaryHandler : MonoBehaviour
{
    [SerializeField] private GameObject turnOn;
    [SerializeField] private TMPro.TMP_Text winnerText;

    [SerializeField] private List<TMPro.TMP_Text> playerWinSummaryText;

    public void ShowSummary()
    {
        turnOn.SetActive(true);

        PlayerInput[] players = GameObject.FindObjectsOfType<PlayerInput>();
        ScoreHandler scoreHandler = GameObject.FindObjectOfType<ScoreHandler>();
        var winningScore = scoreHandler.WinningPlayer(out List<int> winners);
        if (winners.Count == 1)
        {
            winnerText.text = $"Winner is Player {winners[0] + 1}";
        } 
        else
        {
            winnerText.text = $"Result is a tie!";
        }

        List<PlayerInput> currentPlayers = new List<PlayerInput>(players);
        for(int i = 0; i < playerWinSummaryText.Count && i < players.Length; i++)
        {
            var player = currentPlayers.Find(x => x.playerIndex == i);
            var text = playerWinSummaryText[i];
            int playerKills = scoreHandler.NumberOfKills(i);
            text.gameObject.SetActive(true);
            text.text = $"Player {i + 1}: \n {playerKills} kills \n {0} wins";
        }
    }
}
