using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneTransfer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject[] objectsToDestroy;
    public void ReloadScene()
    {
        foreach (GameObject o in objectsToDestroy)
        {
            DestroyImmediate(o);
        }
        Invoke("LoadScene", 1f);
    }
    private void LoadScene()
    {
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex == 0) ? 1 : 0);
    }
}
