using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    
    public void StartGame(bool godMode)
    {
        SceneManager.LoadScene("LevelSelector");
        LevelSelector.GodMode = godMode;
        // gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ResetSaves()
    {
        PlayerPrefs.DeleteAll();
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}
