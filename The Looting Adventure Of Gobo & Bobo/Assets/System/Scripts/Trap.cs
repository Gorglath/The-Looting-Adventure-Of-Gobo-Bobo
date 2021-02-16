using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [HideInInspector]
    public bool IsTrapActive = false;
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenTrapActivated;
    [Header("Variables")]
    [SerializeField]
    private Vector3 amountToRaiseSpikes = Vector3.up;
    [SerializeField]
    private float timeBeforeActivatingSpikes = 3f;
    [SerializeField]
    private float timeBeforeRetractingSpikes = 3f;

    //helpers
    private bool didAlreadyActivatedTrap = false;
    private Vector3 initialPosition = Vector3.zero;
    private void Start()
    {
        initialPosition = transform.localPosition;
    }
    public void ActivateTrap()
    {
        if (!didAlreadyActivatedTrap)
        {
            didAlreadyActivatedTrap = true;
            EventManager.Instance.InvokeTimedEvents(whenTrapActivated);
            //TODO: Add warning fx.
            Tween.LocalPosition(transform, initialPosition + amountToRaiseSpikes, 0.3f, timeBeforeActivatingSpikes, null, Tween.LoopType.None, null, ActivatedTrap);
            Tween.LocalPosition(transform, initialPosition, 1f, timeBeforeRetractingSpikes + timeBeforeActivatingSpikes,null,Tween.LoopType.None,null,ResetAlreadyActivatedTrap);
        }
    }
    private void ActivatedTrap()
    {
        IsTrapActive = true;
    }
    private void ResetAlreadyActivatedTrap()
    {
        didAlreadyActivatedTrap = false;
        IsTrapActive = false;
    }
}
