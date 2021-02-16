using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAreasManager : Singleton<SwitchAreasManager>
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenSwitchingToVendorArea;
    [SerializeField]
    private TimedEvent[] whenSwitchinToLootingArea;

    //helpers
    private bool isCurrentlyAtVendorArea = true;
    public void SwitchToVendorArea()
    {
        if (!isCurrentlyAtVendorArea)
        {
            isCurrentlyAtVendorArea = true;
            Debug.Log("SwitchedToVendorArea");
            EventManager.Instance.InvokeTimedEvents(whenSwitchingToVendorArea);
        }
    }
    private void OnDestroy()
    {
        _instance = null;
    }
    public void SwitchToLootingArea()
    {
        if (isCurrentlyAtVendorArea)
        {
            isCurrentlyAtVendorArea = false;
            Debug.Log("SwitchedToLootingArea");
            EventManager.Instance.InvokeTimedEvents(whenSwitchinToLootingArea);
        }
    }
}
