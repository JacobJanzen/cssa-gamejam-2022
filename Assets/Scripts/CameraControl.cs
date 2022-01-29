using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float baseAltitude = 0;
    private float sceneHeight;
    private Material bgMaterial;
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
        Debug.Log($"Scene height: {sceneHeight}");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y + 1, offset.z); // Camera follows the player with specified offset position
        
        bgMaterial.SetFloat("_Altitude", player.position.y + baseAltitude);
        bgMaterial.SetFloat("_PlayerX", player.position.x);
        bgMaterial.SetFloat("_MaxAltitude", sceneHeight + baseAltitude);
    }
}
