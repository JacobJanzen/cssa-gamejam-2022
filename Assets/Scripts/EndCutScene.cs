using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCutScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine("LoadEndSceneAfter", 27);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            LoadEndScene();
        }
    }

    IEnumerator LoadEndSceneAfter(float time)
    {
        yield return new WaitForSeconds(time);
        LoadEndScene();
    }

    void LoadEndScene()
    {
        SceneManager.LoadScene("EndScene");
    }
}
