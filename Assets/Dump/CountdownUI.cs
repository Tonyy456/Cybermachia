using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private float defaultTimeLength;
    [SerializeField] private float startDelay;
    [SerializeField] private TMPro.TMP_Text textToUpdate;
    [SerializeField] private UnityEvent onTimerDone;
    [SerializeField] private string formatString = "{0:00}:{1:00}";

    private IEnumerator routine;

    public void StartTimer()
    {
        StartTimer(defaultTimeLength);
    }

    public void StartTimer(float time)
    {
        if(routine != null) StopCoroutine(routine);
        routine = textToUpdate == null ?
            TimerRoutine(time) : TimerTextRoutine(time, startDelay);

        StartCoroutine(routine);
    }

    public IEnumerator TimerTextRoutine(float lengthOfTime, float startDelay = 0f)
    {
        textToUpdate.text = $"{FormatSecondsAsMMSS(lengthOfTime)}";
        yield return new WaitForSeconds(startDelay);

        float startTime = Time.time;
        while((Time.time - startTime) < lengthOfTime)
        {
            //evaluate time left on timer
            textToUpdate.text = $"{FormatSecondsAsMMSS(lengthOfTime - (Time.time - startTime))}";
            yield return new WaitForEndOfFrame();
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
        string formattedTime = string.Format(formatString, minutes, seconds);
        return formattedTime;
    }
}
