using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        SingletonAwake();
    }

    void Start()
    {
        FindFirstObjectByType<DeathZone>().deathEvent += () => Debug.Log("Yo");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int value) 
    {
        score += value;
    }
}
