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
    [SerializeField] private TMPro.TMP_Text title;
    [SerializeField] private Transform textParentScreenOne;
    [SerializeField] private UnityEvent onMenusExhausted;
    private int currentMenu = 0;
    public void ShowNext()
    {
        switch(currentMenu)
        {
            case 0:
                ShowKillCounts();
                break;
            case 1:
                ShowHitCount();
                break;
            case 2:
                ShowWinner();
                break;
            default:
                onMenusExhausted?.Invoke();  
                break;
        }
        currentMenu++;
    }

    private void ShowKillCounts()
    {
        title.text = "Number of Kills";
        int playerCount = PlayerConnector.numPlayers;
        for (int i = 0; i < textParentScreenOne.childCount; i++) textParentScreenOne.GetChild(i).gameObject.SetActive(false);

        List<(int player, int kills)> sortedKills = new List<(int player, int kills)>();
        for (int i = 0; i < playerCount; i++)
        {
            var stats = statTracker.GetStatistics(i);
            sortedKills.Add((i, stats.PlayersKilled));
        }
        sortedKills.OrderByDescending(x => x.kills).ToList();

        for(int i = 0; i < sortedKills.Count; i++)
        {
            var text = textParentScreenOne.GetChild(i).GetComponent<TMPro.TMP_Text>();
            var statTuple = sortedKills[i];
            text.text = $"Player {statTuple.player}: {statTuple.kills}";
            text.gameObject.SetActive(true);
        }
    }

    private void ShowHitCount()
    {
        title.text = "Number of Hits";
        int playerCount = PlayerConnector.numPlayers;
        for (int i = 0; i < textParentScreenOne.childCount; i++) textParentScreenOne.GetChild(i).gameObject.SetActive(false);

        List<(int player, int kills)> sortedKills = new List<(int player, int kills)>();
        for (int i = 0; i < playerCount; i++)
        {
            var stats = statTracker.GetStatistics(i);
            sortedKills.Add((i, stats.BulletsHit));
        }
        sortedKills.OrderByDescending(x => x.kills).ToList();

        for (int i = 0; i < sortedKills.Count; i++)
        {
            var text = textParentScreenOne.GetChild(i).GetComponent<TMPro.TMP_Text>();
            var statTuple = sortedKills[i];
            text.text = $"Player {statTuple.player}: {statTuple.kills}";
            text.gameObject.SetActive(true);
        }
    }

    private void ShowWinner()
    {
        title.text = "Winner!";
        for (int i = 0; i < textParentScreenOne.childCount; i++) textParentScreenOne.GetChild(i).gameObject.SetActive(false);
        var text = textParentScreenOne.GetChild(0).GetComponent<TMPro.TMP_Text>();
        text.gameObject.SetActive(true);
        text.text = $"Player {statTracker.getWinner()}";
    }
}
