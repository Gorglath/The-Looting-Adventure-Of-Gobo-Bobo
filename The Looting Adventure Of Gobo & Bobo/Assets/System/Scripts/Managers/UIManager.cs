using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("References")]
    [SerializeField]
    private Transform uiCanvas;
    private void OnDestroy()
    {
        _instance = null;
    }
    //helpers
    public void OpenPanel(int index)
    {
        uiCanvas.GetChild(index).gameObject.SetActive(true);
    }
    public void ClosePanel(int index)
    {
        uiCanvas.GetChild(index).gameObject.SetActive(false);
    }
}
