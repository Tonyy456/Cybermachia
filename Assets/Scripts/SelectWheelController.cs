using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectWheelController : MonoBehaviour
{
    [SerializeField] private List<Minigame> gamesToHave;
    [SerializeField] private List<string> gibberash;

    [SerializeField] private TMPro.TMP_Text left;
    [SerializeField] private TMPro.TMP_Text center;
    [SerializeField] private TMPro.TMP_Text right;

    [Range(0.1f, 10f)]
    [SerializeField] private int lowerBoundOfItems = 10;
    [SerializeField] private int upperBoundOfItems = 20;
    [SerializeField] private float spinSpeed = 10f;
    [SerializeField] private float deaccelerationConst = 10f;

    private Minigame selected;

    public void Start()
    {
        StartWheel();
    }

    public void StartWheel()
    {
        if (gamesToHave.Count < 0) return;
        int index = Random.Range(0, gamesToHave.Count);
        selected = gamesToHave[index];
        StartCoroutine(WheelRoutine());
    }

    private IEnumerator WheelRoutine()
    {
        List<string> wheel = new List<string>();
        List<Color> wheelColor = new List<Color>();
        int numItems = Random.Range(lowerBoundOfItems, upperBoundOfItems);
        int winnerIndex = Random.Range(0, numItems);
        bool gibberashHasItems = gibberash.Count > 0;
        //populate with winner and other random stuff.
        for (int i = 0; i < numItems; i++)
        {

            bool useGibberash = gibberashHasItems && !(Random.Range(0, 10-gamesToHave.Count) == 0);
            if (!useGibberash)
            {
                Minigame game = gamesToHave[Random.Range(0, gamesToHave.Count)];
                wheel.Add(game.minigameName);
                wheelColor.Add(game.displayColor);
            }
            else
            {
                wheel.Add(gibberash[Random.Range(0, gibberash.Count)]);
                wheelColor.Add(Color.white);
            }

        }
        wheel[winnerIndex] = selected.minigameName;

        Debug.Log(selected.minigameName);
        //Rotate through one full time and then slow down as you approach the correct item. On arrival load scene.
        for(int i = 0; i < numItems + winnerIndex + 1; i++)
        {
            // iterate
            int centerIndex = i % wheel.Count;
            left.text = wheel[(centerIndex + 1) % wheel.Count];
            left.color = wheelColor[(centerIndex + 1) % wheel.Count];

            center.text = wheel[centerIndex];
            center.color = wheelColor[centerIndex];

            right.text = wheel[ (centerIndex - 1 + wheel.Count) % wheel.Count];
            right.color = wheelColor[(centerIndex - 1 + wheel.Count) % wheel.Count];

            yield return new WaitForSeconds((1 + (i / numItems) * deaccelerationConst) / spinSpeed);
        }
        yield return null;
    }
}
