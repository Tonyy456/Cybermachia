using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;

public class TargetShooterGameController : IPlayerConnectedHandler
{
    [Header("Score")]
    [SerializeField] private List<RoundBoardController> scoreControllers;

    [Header("Events")]
    [SerializeField] private UnityEvent onGameOver;

    // Private variables
    private List<PlayerInput> players = new List<PlayerInput>();

    public void TriggerGameOver()
    {
        onGameOver?.Invoke();
    }
    public override void ConnectPlayer(PlayerInput input)
    {
        players.Add(input);
        if (input.playerIndex < scoreControllers.Count)
        {
            scoreControllers[input.playerIndex].AssignToPlayer(input);
        }
        else
        {
            throw new System.Exception("Failed to provide score board controller to player");
        }
    }

    private void OnPlayerScore(PlayerInput player, int difference)
    {
        scoreControllers[player.playerIndex].AddToScore(difference);
    }
}
