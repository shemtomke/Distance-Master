using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameTimer : MonoBehaviour
{
    public float timer;
    private bool isRunning;
    public Text timerText;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;
        }

        timerText.text = GetTimerString();
    }

    public void StartTimer()
    {
        timer = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public string GetTimerString()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        string timerString = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        return timerString;
    }
}
