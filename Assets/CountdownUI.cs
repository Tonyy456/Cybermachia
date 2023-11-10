using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private float defaultTimeLength;
    [SerializeField] private TMPro.TMP_Text textToUpdate;
    [SerializeField] private UnityEvent onTimerDone;

    private IEnumerator routine;

    public void StartTimer()
    {
        StartTimer(defaultTimeLength);
    }

    public void StartTimer(float time)
    {
        if(routine != null) StopCoroutine(routine);
        routine = textToUpdate == null ?
            TimerRoutine(time) : TimerTextRoutine(time);

        StartCoroutine(routine);
    }

    public IEnumerator TimerTextRoutine(float lengthOfTime)
    {
        float startTime = Time.time;
        float timePassed = Time.time - startTime;
        float timeLeft = lengthOfTime - timePassed;
        while((timePassed) < lengthOfTime)
        {
            // update ui
            textToUpdate.text = $"{FormatSecondsAsMMSS(timeLeft)}";

            yield return new WaitForEndOfFrame();
            timePassed = Time.time - startTime;
            // update time
        }

        onTimerDone?.Invoke();
        yield return null;
    }
    public IEnumerator TimerRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        onTimerDone?.Invoke();
        yield return null;
    }

    private string FormatSecondsAsMMSS(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds % 60f);
        string formattedTime = string.Format("{0:00}:{1:00}", minutes, seconds);
        return formattedTime;
    }
}
