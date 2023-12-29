using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float timer;
    public Text timerText;
    public Text customTimeText;

    [Header("Timer")]
    public float easyTimer;
    public float mediumTimer;
    public float hardTimer;
    public float customTimer;

    GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        GetTimer(gameManager.currentDifficulty);
    }
    private void Update()
    {
        TimeLapse();
    }
    // Each Difficulty has a time period
    // Custom Mode: Players can set their own time limit for measuring the distance.
    void GetTimer(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                timer = easyTimer; // 10 Minutes
                break;
            case Difficulty.Medium:
                timer = mediumTimer;
                break;
            case Difficulty.Hard:
                timer = hardTimer;
                break;
            case Difficulty.Custom: //Customize
                timer = customTimer;
                break;
        }
    }

    void TimeLapse()
    {
        if (!gameManager.isStart)
            return;

        // Update the timer with the elapsed time
        timer -= Time.deltaTime;

        // Check if the timer has reached zero
        if (timer <= 0f)
        {
            timer = 0f; // Clamp the timer to zero to avoid negative values
            gameManager.isGameOver = true;
        }

        // Update the UI text to display the current timer value
        CountdownTimer();
    }
    // CountDown Time
    void CountdownTimer()
    {
        // Convert the timer value to minutes and seconds
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        // Update the UI text
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
