using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    #region Singleton
    public static DontDestroyOnLoad Instance { get; private set; }

    private void SingletonAwake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion

    private void Awake()
    {
        SingletonAwake();
        DontDestroyOnLoad(this);
    }
}