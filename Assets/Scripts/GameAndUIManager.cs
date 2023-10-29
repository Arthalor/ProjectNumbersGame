using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.SearchService;

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

    private void Awake()
    {
        SingletonAwake();
    }

    void Start()
    {
        FindFirstObjectByType<DeathZone>().deathEvent += OnGameLost;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score:\n" + score;
    }

    private void OnGameLost() 
    {
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
}
