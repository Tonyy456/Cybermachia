using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWheelController : MonoBehaviour
{
    [SerializeField] private List<Minigame> gamesToHave;
    [SerializeField] private List<string> gibberash;
    [SerializeField] private int numItemsCycled = 10;

    private Minigame selected;
    public void StartWheel()
    {
        if (gamesToHave.Count < 0) return;
        int index = Random.Range(0, gamesToHave.Count - 1);
        selected = gamesToHave[index];
        StartCoroutine(WheelRoutine());
    }

    public IEnumerator WheelRoutine()
    {

        yield return null;
    }
}
