using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openDinoLink()
    {
        Application.OpenURL("https://www.dinosaurearthsociety.com/faq.php");
    }

    public void openMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Game Started");
    }
}
