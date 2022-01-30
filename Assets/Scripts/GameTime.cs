using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_Object;
    private const float MAX_ALTITUDE = 250.0f;//min alt is assumed to be 0
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
        m_Object.text = "0:00'00";
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        int minutes = (int)time/60;
        int seconds = (int)time%60;
        int milliseconds = (int)(time*100)%100;

        string minuteString = minutes.ToString();
        if(minutes < 10)
        {
            minuteString = 0 + minuteString;
        }

        string secondString = seconds.ToString();
        if (seconds < 10)
        {
            secondString = 0 + secondString;
        }

        string millisecondString = milliseconds.ToString();
        if (milliseconds < 10)
        {
            millisecondString = 0 + millisecondString;
        }

        m_Object.text = minuteString + ":" + secondString + "'" + millisecondString;
    }
}
