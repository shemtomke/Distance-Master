using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Implement varying time limits for different difficulty levels (Easy, Medium, Hard), with a customizable option for 'Custom'.
    public bool isStart = false; // after selecting an image
    public bool isGameOver = false;
    public Difficulty currentDifficulty;

    private void Start()
    {
        currentDifficulty = Difficulty.Easy;
    }
    // Take A Screenshot
    public void TakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot("Measure Distance " + System.DateTime.Now.ToString("MM-dd-yy (HH-mm-ss)") + ".png");
    }
}
