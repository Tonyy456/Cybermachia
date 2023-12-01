using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;

public class TargetShooterGameController : IPlayerConnectedHandler
{
    [Header("Score")]
    [SerializeField] private MaterialHolder playerColors;
    [SerializeField] private TMPro.TMP_Text winnerText;
    [SerializeField] private string formatString;
    [SerializeField] private List<RoundBoardController> scoreControllers;

    [Header("Events")]
    [SerializeField] private UnityEvent onGameOver;

    // Private variables
    private List<PlayerInput> players = new List<PlayerInput>();

    public void TriggerGameOver()
    {
        HandleGameOver();
        onGameOver?.Invoke();
    }
    public override void ConnectPlayer(PlayerInput input)
    {
        players.Add(input);
        if (input.playerIndex < scoreControllers.Count)
        {
            scoreControllers[input.playerIndex].gameObject.SetActive(true);
            scoreControllers[input.playerIndex].AssignToPlayer(input);
        }
        else
        {
            throw new System.Exception("Failed to provide score board controller to player");
        }
    }

    public void OnPlayerScore(PlayerInput player, int difference)
    {
        scoreControllers[player.playerIndex].AddToScore(difference);
    }

    public void HandleGameOver()
    {
        TargetShooterPlayer[] players = GameObject.FindObjectsOfType<TargetShooterPlayer>();
        foreach(var x in players)
        {
            x.AllowedToShoot = false;
        }
        int winnerIndex = -1;
        int winningScore = -1;
        foreach(var board in scoreControllers)
        {
            if (board.PlayerNumber < 0) continue;
            if (board.Score > winningScore)
            {
                winnerIndex = board.PlayerNumber;
            }
        }
        winnerText.text = string.Format(formatString, winnerIndex, winningScore);
        Color toUse = playerColors.playerMaterials[winnerIndex - 1].GetColor("_OutlineColor");
        toUse.a = 1;
        winnerText.color = toUse;
    }
}
