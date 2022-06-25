using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenuController : MonoBehaviour
{
    public void LoadLevelSelector()
    {
        SceneManager.LoadScene(1);
    }
    
    public void LoadNextLevel(int levelIdx)
    {
        if (levelIdx == 0)
        {
            levelIdx = SceneManager.GetActiveScene().buildIndex + 1;
            levelIdx = levelIdx >= SceneManager.sceneCountInBuildSettings ? 0 : levelIdx;
        }
        SceneManager.LoadScene(levelIdx);
    }
    
    public void Quit()
    {
        Application.Quit();
    }
}
