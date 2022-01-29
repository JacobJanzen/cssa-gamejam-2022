using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraControl : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public float maxHeight;
    // Start is called before the first frame update
    void Start()
    {
        Skybox skybox = this.GetComponent<Skybox>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y + 1, offset.z); // Camera follows the player with specified offset position
        Skybox skybox = this.GetComponent<Skybox>();
        skybox.material.SetFloat("_Altitude", player.position.y);
        skybox.material.SetFloat("_PlayerX", player.position.x);
        skybox.material.SetFloat("_MaxAltitude", 27f);
    }
}
