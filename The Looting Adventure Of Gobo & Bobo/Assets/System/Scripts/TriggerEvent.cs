using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] eventToTrigger;

    [Header("Variables")]
    [SerializeField]
    private bool isRepeatable = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.InvokeTimedEvents(eventToTrigger);
            if (!isRepeatable)
            {
                Destroy(gameObject);
            }
        }
    }
}
