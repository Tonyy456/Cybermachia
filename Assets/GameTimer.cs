using System;
using System.Collections;
using System.Collections.Generic;
using Tony;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private StateSO startOn;
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private float gameTime;
    [SerializeField] private UnityEvent onTimerFinished;

    private IEnumerator routine;
    public void Start()
    {
        startOn.OnEnter += StartTimer;
    }

    public void StartTimer()
    {
        routine = TimerRoutine(gameTime);
        StartCoroutine(routine);
    }

    public IEnumerator TimerRoutine(float duration)
    {
        float secondsLeft = duration;
        while(secondsLeft > 0)
        {
            yield return new WaitForEndOfFrame();
            secondsLeft -= Time.deltaTime;
            SetText(secondsLeft);
        }
        onTimerFinished?.Invoke();
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
