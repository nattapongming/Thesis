using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using Manager;

public class InGameAnalytics : MonoBehaviour
{
    // Start is called before the first frame update

    GameManager gameManager;
    LevelStatManager levelStatManager;
    void Start()
    {
        Initialize();
        gameManager = GameManager.Instance;
        if (gameManager) levelStatManager = gameManager.levelStatManager;

        /*OnStartPlayLevel();
        OnFinishPlayLevel();
        PlayTimeInLevel();*/
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("4"))
        {
            OnStartPlayLevel();
        }

        if (Input.GetKeyDown("5"))
        {
            OnFinishPlayLevel();
        }

        if (Input.GetKeyDown("6"))
        {
            PlayTimeInLevel();
        }
    }

    private async void Initialize()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }

    public void RunAnalytic()
    {
        /*OnStartPlayLevel();
        OnFinishPlayLevel();
        PlayTimeInLevel();*/
    }

    public void OnStartPlayLevel()
    {
        CustomEvent playerPlayLevel = new CustomEvent("playerPlayLevel")
        {
            { "difficulty",gameManager.curDifficulty.ToString()}, { "levelName", levelStatManager.levelName}
        };

        AnalyticsService.Instance.RecordEvent(playerPlayLevel);
        Debug.Log($"Record player play \"{levelStatManager.levelName}\" at {gameManager.curDifficulty} Difficulty!");
    }

    public void OnFinishPlayLevel()
    {
        CustomEvent playerFinishLevel = new CustomEvent("playerFinishLevel")
        {
            { "difficulty",gameManager.curDifficulty.ToString()}, { "levelName", levelStatManager.levelName}
        };

        AnalyticsService.Instance.RecordEvent(playerFinishLevel);
        Debug.Log($"Record player finish \"{levelStatManager.levelName}\" at {gameManager.curDifficulty} Difficulty!");
    }

    public void PlayTimeInLevel()
    {
        int totalTimeBySec = Mathf.FloorToInt((levelStatManager.minutes * 60) + levelStatManager.seconds);

        CustomEvent inLevelPlayTime = new CustomEvent("inLevelPlayTime")
        {
            { "difficulty",gameManager.curDifficulty.ToString()}, { "levelName", levelStatManager.levelName}, { "timeByTotalSeconds", totalTimeBySec}
        };

        AnalyticsService.Instance.RecordEvent(inLevelPlayTime);
        Debug.Log($"Record player playtime in \"{levelStatManager.levelName}\" at {gameManager.curDifficulty} Difficulty and {totalTimeBySec} seconds playtime!");
    }
}
