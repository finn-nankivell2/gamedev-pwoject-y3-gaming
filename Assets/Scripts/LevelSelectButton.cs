using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public int levelNum;
    public TMP_Text bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        float savedTime = PlayerPrefs.GetFloat("Level" + levelNum + "Time", -1);
        if (savedTime == -1) {
            bestTimeText.text = "No time set";
        }
        else {
            TimeSpan time = TimeSpan.FromSeconds(savedTime);
            bestTimeText.text = "Best time: " + time.Minutes.ToString() + ":" + time.Seconds.ToString("D2") + ":" + time.Milliseconds.ToString("D3");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
