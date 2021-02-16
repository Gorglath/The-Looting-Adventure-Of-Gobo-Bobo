using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : Singleton<Timer>
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenTimerStarts;
    [SerializeField]
    private TimedEvent[] whenTimerEnds;

    [Header("References")]
    [SerializeField]
    private TMP_Text timerText = null;

    [Header("Variables")]
    [SerializeField]
    private float maxTime = 60f;

    //helpers
    private float bonusTime = 0f;
    private bool didAlreadyStart = false;
    private void OnDestroy()
    {
        _instance = null;
    }
    public void StartTimer()
    {
        if (!didAlreadyStart)
        {
            didAlreadyStart = true;
            EventManager.Instance.InvokeTimedEvents(whenTimerStarts);
            StartCoroutine(CountDown(maxTime));
        }
    }
    public void AddBonusTime(float amount)
    {
        bonusTime += amount;
    }
    private void AssignTimeText(float currentTime)
    {
        currentTime += bonusTime;
        timerText.text = currentTime.ToString().Substring(0,3);
    }
    IEnumerator CountDown(float currentTime)
    {
        if(currentTime > 0f - bonusTime)
        {
            currentTime -= Time.deltaTime;
            AssignTimeText(currentTime);
            yield return null;
            StartCoroutine(CountDown(currentTime));
        }
        else
        {
            EventManager.Instance.InvokeTimedEvents(whenTimerEnds);
        }
    }
}
