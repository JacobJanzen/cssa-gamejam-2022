using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraControl : MonoBehaviour
{
    private const float MIN_SIZE = 6f;
    private const float MAX_SIZE = 50f;
    private const float MAX_ALTITUDE = 250.0f;//min alt is assumed to be 0
    public Transform player;
    public Vector3 offset;
    public float baseAltitude = 0;
    private float sceneHeight;
    private Material bgMaterial;
    public Camera m_OrthographicCamera;
    // Start is called before the first frame update
    void Start()
    {
        bgMaterial = this.GetComponent<Skybox>().material;
        // Calculate the scene's bounds 
        Bounds b = new Bounds();
        foreach (Renderer r in FindObjectsOfType<Renderer>())
        {
            b.Encapsulate(r.bounds);
        }
        sceneHeight = b.max.y;

        //If the Camera exists in the inspector, enable orthographic mode and change the size
        if (m_OrthographicCamera)
        {
            //This enables the orthographic mode
            m_OrthographicCamera.orthographic = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (player.position.y >= sceneHeight)
        {
            SceneManager.LoadScene("EndCutscene");
            return;
        }
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y + 1, offset.z); // Camera follows the player with specified offset position

        bgMaterial.SetFloat("_Altitude", player.position.y + baseAltitude);
        bgMaterial.SetFloat("_PlayerX", player.position.x);
        bgMaterial.SetFloat("_MaxAltitude", sceneHeight + baseAltitude);
        updateCamera();
    }

    void updateCamera()
    {
        this.m_OrthographicCamera.orthographicSize = MIN_SIZE + ((player.position.y / MAX_ALTITUDE) * MAX_SIZE);
    }
}
