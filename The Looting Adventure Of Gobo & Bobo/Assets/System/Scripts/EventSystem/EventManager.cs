using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    private void OnDestroy()
    {
        _instance = null;
    }
    public void InvokeTimedEvents(TimedEvent[] timedEvents)
    {
        StartCoroutine(InvokeEvents(timedEvents));
    }
    IEnumerator InvokeEvents(TimedEvent[] timedEvent)
    {
        foreach (TimedEvent eventToInvoke in timedEvent)
        {
            yield return new WaitForSeconds(eventToInvoke.TimeBeforeInvokingEvent);
            eventToInvoke.Event.Invoke();
        }
    }
}
