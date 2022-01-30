using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("CombinedPlatforms");
        Debug.Log("Game Started");
    }

    public void QuitGame()
    {
        Application.Quit();
        Application.OpenURL("about:blank");
        Debug.Log("Game Exited");
    }
}
