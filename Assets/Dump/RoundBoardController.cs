using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoundBoardController : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text roundScore;
    [SerializeField] private string scoreFormatString;
    [SerializeField] private TMPro.TMP_Text playerText;
    [SerializeField] private string playerTextFormatString;

    private int score = 0;
    public int Score
    {
        get
        {
            return score;
        }
        private set
        {
            score = value;
            if (roundScore) roundScore.text = string.Format(scoreFormatString, score);
        }
    }

    private PlayerInput player;
    public int PlayerNumber
    {
        get
        {
            if (player == null) return -1;
            return player.playerIndex + 1;
        }
    }

    public void AssignToPlayer(PlayerInput input)
    {
        player = input;
        if (playerText) playerText.text = string.Format(playerTextFormatString, PlayerNumber);
        Score = 0;
    }

    public void AddToScore(int difference)
    {
        Score += difference;
    }
}
