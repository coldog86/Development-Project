using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    [Header("Component")]
    public RoundData[] allRoundData;
    
    [Header("Setting")]
    public int menuSceneIndex = 1;
    public string highestScoreKey = "HighestScore";

    public string scoreKey { get { return highestScoreKey; } }

    private PlayerProgress _playerProgress;
    private string _gameDataFileName = "data.json";

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        LoadGameData();
        LoadPlayerProgress();
        SceneManager.LoadScene(menuSceneIndex, LoadSceneMode.Single);
    }

    public RoundData GetCurrentRoundData()
    {
        return allRoundData[0];
    }

    private void LoadGameData()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, _gameDataFileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            GameData loadedData = JsonUtility.FromJson<GameData>(jsonData);

            if (loadedData != null)
                allRoundData = loadedData.allRoundData;
            else
                Debug.LogError("DataContoller : LoadGameData() - Could not find data to load.");
               
        }
        else
            Debug.LogError("DataContoller : LoadGameData() - Could not find data file.");

    }

    public void SetPlayerHighestScore(int score)
    {
        if (score > _playerProgress.highestScore)
        {
            _playerProgress.highestScore = score;
            SavePlayerProgress();
        }
    }

    public int GetPlayerHighestScore()
    {
        return _playerProgress.highestScore;
    }

    private void LoadPlayerProgress()
    {
        _playerProgress = new PlayerProgress();

        if (PlayerPrefs.HasKey(highestScoreKey))
            _playerProgress.highestScore = PlayerPrefs.GetInt(highestScoreKey);
    }

    private void SavePlayerProgress()
    {
        PlayerPrefs.SetInt(highestScoreKey, _playerProgress.highestScore);
    }
}