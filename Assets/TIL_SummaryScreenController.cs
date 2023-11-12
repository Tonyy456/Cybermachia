using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIL_SummaryScreenController : MonoBehaviour
{
    [SerializeField] private TIL_GameStatistics statTracker;
    [SerializeField] private Transform textParentScreenOne;
    public void Show()
    {
        // part 1
        for(int i = 0; i < textParentScreenOne.childCount; i++)
        {
            var stats = statTracker.GetStatistics(i);
            var child = textParentScreenOne.GetChild(i);
            var text = child.GetComponent<TMPro.TMP_Text>();
            text.text = $"Player {i}: {stats.PlayersKilled}";
        }
    }
}
