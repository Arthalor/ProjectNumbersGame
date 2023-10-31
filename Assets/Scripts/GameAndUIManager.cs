using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;
using System.Collections.Generic;

public class GameAndUIManager : MonoBehaviour
{
    #region Singleton
    public static GameAndUIManager Instance { get; private set; }

    private void SingletonAwake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    [SerializeField] private int score = 0;
    [SerializeField] private TextMeshProUGUI scoreText;
    [Space]
    [SerializeField] private GameObject loseScreenUI;

    [SerializeField] private string saveFilePath;
    [SerializeField] private Highscores highscoreData;
    [SerializeField] private List<TextMeshProUGUI> highscoreTexts;

    private void Awake()
    {
        SingletonAwake();
        saveFilePath = Application.persistentDataPath + "/highscore.data";
    }

    void Start()
    {
        FindFirstObjectByType<DeathZone>().deathEvent += OnGameLost;
        if (!DoesHighscoreFileExist())
        {
            highscoreData = new();
            InitializeHighscores(highscoreData);
            WriteJsonHighscores();
        }
        else
        {
            ReadJsonHighscores();
            InitializeHighscores(highscoreData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score:\n" + score;
    }

    private void OnGameLost()
    {
        highscoreData.highscoreList.Add(score);
        WriteJsonHighscores();
        loseScreenUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void UpdateScore(int value)
    {
        score += value;
    }

    private bool DoesHighscoreFileExist()
    {
        return File.Exists(saveFilePath);
    }

    private void InitializeHighscores(Highscores highscores)
    {
        for (int i = 0; i < 3; i++) 
        {
            highscoreTexts[i].text = PlacementString(i + 1, highscores.highscoreList[i]);
        }
        //highscoreTexts[3].text = PlacementString(highscores.highscoreList.Count, score);
    }

    private string PlacementString(int place, int score)
    {
        return "<align=left>" + place + ":<line-height=0>\n<align=right>" + score + "<line-height=1em>";
    }

    private void ReadJsonHighscores() 
    {
        string jsonData = File.ReadAllText(saveFilePath);
        highscoreData.LoadFromJSON(jsonData);
    }

    public void WriteJsonHighscores()
    {
        highscoreData.Sort();
        highscoreData.RemoveDuplicates();
        string jsonString = JsonUtility.ToJson(highscoreData);
        File.WriteAllText(saveFilePath, jsonString);
    }

    private void OnApplicationQuit()
    {
        highscoreData.highscoreList.Add(score);
        WriteJsonHighscores();
    }

    [Serializable]
    private class Highscores 
    {
        public List<int> highscoreList;

        public Highscores() 
        {
            highscoreList = new List<int> { 0, 0, 0 };
        }

        public void Sort()
        {
            highscoreList.Sort((a, b) => b.CompareTo(a)); // descending sort
        }

        public void RemoveDuplicates() 
        {
            int previous = 0;
            for (int i = 0; i < highscoreList.Count; i++) 
            {
                if (previous == highscoreList[i] && previous != 0)
                {
                    highscoreList.RemoveAt(i);
                }
                else previous = highscoreList[i];
            }
        }

        public void LoadFromJSON(string jsonString)
        {
            JsonUtility.FromJsonOverwrite(jsonString, this);
        }
    }
}
