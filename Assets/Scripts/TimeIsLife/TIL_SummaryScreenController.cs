using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TIL_SummaryScreenController : MonoBehaviour
{
    [SerializeField] private TIL_GameStatistics statTracker;
    [SerializeField] private Transform KillCountTextParent;
    [SerializeField] private Transform HitCountTextParent;
    [SerializeField] private Transform WinnerTextParent;
    public void Show()
    {
        ShowHitCount();
        ShowKillCounts();
        ShowWinner();
    }

    private void ShowKillCounts()
    {
        int playerCount = PlayerConnector.numPlayers;
        for (int i = 0; i < KillCountTextParent.childCount; i++) KillCountTextParent.GetChild(i).gameObject.SetActive(false);

        List<(int player, int kills)> sortedKills = new List<(int player, int kills)>();
        for (int i = 0; i < playerCount; i++)
        {
            var stats = statTracker.GetStatistics(i);
            sortedKills.Add((i, stats.PlayersKilled));
        }
        sortedKills.OrderByDescending(x => x.kills).ToList();

        for(int i = 0; i < sortedKills.Count; i++)
        {
            var text = KillCountTextParent.GetChild(i).GetComponent<TMPro.TMP_Text>();
            var statTuple = sortedKills[i];
            text.text = $"Player {statTuple.player + 1}: {statTuple.kills}";
            text.gameObject.SetActive(true);
        }
    }

    private void ShowHitCount()
    {
        int playerCount = PlayerConnector.numPlayers;
        for (int i = 0; i < HitCountTextParent.childCount; i++) HitCountTextParent.GetChild(i).gameObject.SetActive(false);

        List<(int player, int kills)> sortedKills = new List<(int player, int kills)>();
        for (int i = 0; i < playerCount; i++)
        {
            var stats = statTracker.GetStatistics(i);
            sortedKills.Add((i, stats.BulletsHit));
        }
        sortedKills.OrderByDescending(x => x.kills).ToList();

        for (int i = 0; i < sortedKills.Count; i++)
        {
            var text = HitCountTextParent.GetChild(i).GetComponent<TMPro.TMP_Text>();
            var statTuple = sortedKills[i];
            text.text = $"Player {statTuple.player}: {statTuple.kills}";
            text.gameObject.SetActive(true);
        }
    }

    private void ShowWinner()
    {
        var text = WinnerTextParent.GetChild(0).GetComponent<TMPro.TMP_Text>();
        text.gameObject.SetActive(true);
        text.text = $"Player {statTracker.getWinner() + 1}";
    }
}
