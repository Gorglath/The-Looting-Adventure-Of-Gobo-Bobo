using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TimedEvent
{
    public float TimeBeforeInvokingEvent = 0f;
    public UnityEvent Event;

    private void Awake()
    {
        Event = new UnityEvent();
    }
}
