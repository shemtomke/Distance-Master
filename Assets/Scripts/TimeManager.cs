using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public float gameTime;
    public Text customTime;

    // Each Difficulty has a time period
    // Custom Mode: Players can set their own time limit for measuring the distance.
    void GetTime(Dififculty difficulty)
    {
        switch (difficulty)
        {
            case Dififculty.Easy:
                gameTime = 10;
                break;
            case Dififculty.Medium:
                gameTime = 5;
                break;
            case Dififculty.Hard:
                gameTime = 2;
                break;
            case Dififculty.Custom: //Customize
                gameTime = 1;
                break;
        }
    }

    void TimeLapse()
    {

    }
    // CountDown Time

}
