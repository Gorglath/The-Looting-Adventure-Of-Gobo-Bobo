using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    [Header("Events")]
    [SerializeField]
    private TimedEvent[] whenAddingScore;
    private void OnDestroy()
    {
        _instance = null;
    }
    [Header("References")]
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text winningScoreText;
    [Header("Variables")]
    [SerializeField]
    private int scoreNeededToAddTime = 500;
    [SerializeField]
    private float timeToAdd = 5f;
    //helpers
    private int currentScore = 0;
    private int currentDivisionAmount = 0;
    private int previousDivisionAmount = 0;
    private void OnEnable()
    {
        scoreText.text = "0";
    }
    public void AddScore(int scoreToAdd)
    {
        
        EventManager.Instance.InvokeTimedEvents(whenAddingScore);
        currentScore += scoreToAdd;
        currentDivisionAmount = Mathf.FloorToInt((float)currentScore / (float)scoreNeededToAddTime);
        if ( currentDivisionAmount > 0 &&
             currentDivisionAmount != previousDivisionAmount)
        {
            Timer.Instance.AddBonusTime(timeToAdd * (currentDivisionAmount - previousDivisionAmount));
            previousDivisionAmount = currentDivisionAmount;
        }
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = currentScore.ToString();
        winningScoreText.text = scoreText.text;
    }
}
