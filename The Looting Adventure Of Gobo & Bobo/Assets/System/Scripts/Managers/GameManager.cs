using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenSceneStarts;
    [SerializeField]
    private TimedEvent[] whenPressedPlay;
    [SerializeField]
    private TimedEvent[] whenRestartingGame;

    [Header("References")]
    [SerializeField]
    private sceneTransfer sceneTransfer;
    void Start()
    {
        if (PlayerPrefs.HasKey("Restart"))
        {
            PlayerPrefs.DeleteKey("Restart");
            StartGame();
            UIManager.Instance.ClosePanel(0);
        }
        EventManager.Instance.InvokeTimedEvents(whenSceneStarts);
    }

    public void StartGame()
    {
        EventManager.Instance.InvokeTimedEvents(whenPressedPlay);
    }
   
    public void ReloadScene()
    {
        sceneTransfer.ReloadScene();
    }
    public void RestartGame()
    {
        PlayerPrefs.SetInt("Restart", 1);
        sceneTransfer.ReloadScene();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
