using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedrunTimer : MonoBehaviour
{
    private float _currentTime;
    private bool _timerActive;
    private TMP_Text _text;

    void Start()
    {
        _currentTime = 0;
        _text = GetComponent<TMP_Text>();
        StartTimer();
    }

    void Update()
    {
        if(_timerActive)
        {
            _currentTime += Time.deltaTime;
        }

        _text.text = GetTime();
    }

    public void StartTimer()
    {
        _timerActive = true;
    }

    public void StopTimer()
    {
        _timerActive = false;
    }

    public string GetTime()
    {
        TimeSpan time = TimeSpan.FromSeconds(_currentTime);
        return time.Minutes.ToString() + ":" + time.Seconds.ToString("D2") + ":" + time.Milliseconds.ToString("D3");
    }
}
