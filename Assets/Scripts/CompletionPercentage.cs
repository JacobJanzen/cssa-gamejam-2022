using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CompletionPercentage : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Object;
    public Transform player;
    private const float MAX_ALTITUDE = 250.0f;//min alt is assumed to be 0

    // Start is called before the first frame update
    void Start()
    {
        m_Object.text = "0% Complete";
    }

    // Update is called once per frame
    void Update()
    {
        int completion = (int)((player.position.y / MAX_ALTITUDE) * 100);
        if (completion > 100)
        {
            completion = 100;
        }

        m_Object.text = completion + "% Complete";
    }
}
