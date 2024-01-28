using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class timerScript : MonoBehaviour
{
    private DateTime currentTime;
    public Text clockText;
    static public float clockMinute, clockSecond, timeRemaining = 3f * 60;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (dataReader.isGameRunning)
        {
            // currentTime = System.DateTime.Now;
            // Debug.Log("CURRENT TIME: " + currentTime);
            // // clockHour = currentTime.Hour;
            // // clockMinute = currentTime.Minute;
            // clockSecond = currentTime.Second;

            // if (clockSecond == 59)
            // {
            //     clockMinute -= 1;
            // }

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                clockMinute = Mathf.FloorToInt(timeRemaining / 60);
                clockSecond = Mathf.FloorToInt(timeRemaining % 60);
            }
            else if (timeRemaining <= 0)
            {
                timeRemaining = 0;
            }
            clockText.text = clockMinute.ToString() + " : " + clockSecond.ToString();
        }
        else if (!dataReader.isGameRunning)
        {
            clockText.text = "1:29";
        }
    }
}
