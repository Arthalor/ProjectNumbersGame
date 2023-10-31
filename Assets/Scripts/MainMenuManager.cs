using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject audioSettingsMenu = default;

    public void PlayGame() 
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void HideSettingsMenus() 
    {
        audioSettingsMenu.SetActive(false);
    }
}