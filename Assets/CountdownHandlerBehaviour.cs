using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tony;

public class CountdownHandlerBehaviour : MonoBehaviour
{
    [SerializeField] private FiniteStateMachineSO fsm;
    [SerializeField] private StateSO countdownState;

    [SerializeField] private GameObject panel;
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private float cdTime = 5f;

    private IEnumerator currentRoutine;

    public void Start()
    {
        countdownState.OnEnter += StartCountdown;
    }

    public void StartCountdown()
    {
        if (currentRoutine == null)
        {
            currentRoutine = countdownRoutine(cdTime);
            StartCoroutine(currentRoutine);
        } 
    }


    public IEnumerator countdownRoutine(float startSeconds)
    {
        float remainingTime = startSeconds;
        while(remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            SetText(remainingTime);
        }
        currentRoutine = null;
        //fsm.Fire(next);
        yield return null;
    }

    public void SetText(float seconds)
    {
        // Format as "mm:ss"
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        string formattedTime = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        text.text = formattedTime;
    }
}
