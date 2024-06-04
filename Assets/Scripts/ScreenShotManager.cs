using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ScreenShotManager : MonoBehaviour
{
    private string screenshotsFolderPath;

    GameManager gameManager;
    UserManager userManager;
    LogsManager logsManager;
    InGameTimer inGameTimer;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        userManager = FindObjectOfType<UserManager>();
        logsManager = FindObjectOfType<LogsManager>();
        inGameTimer = FindObjectOfType<InGameTimer>();

        // Define the screenshots folder path
        screenshotsFolderPath = Path.Combine(Application.persistentDataPath, "Screenshots");
        // Ensure the Screenshots folder exists
        if (!Directory.Exists(screenshotsFolderPath))
        {
            Directory.CreateDirectory(screenshotsFolderPath);
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(String.IsNullOrEmpty(userManager.currentUserName))
            {
                gameManager.Notification("Failed to capture Screenshot! User Must Login/Sign Up!", Color.red);
            }
            else
            {
                TakeScreenShot();
            }
        }
    }
    public void TakeScreenShot()
    {
        StartCoroutine(TakeScreenshot(userManager.currentUserName));
    }
    public IEnumerator TakeScreenshot(string userName)
    {
        // Create the screenshot name using the user name and timestamp
        string timestamp = DateTime.Now.ToString("ddMMyyyy_HHmmss");
        string screenshotName = $"{userName}_{timestamp}.png";
        string screenshotPath = Path.Combine(screenshotsFolderPath, screenshotName);

        // Capture the screenshot
        ScreenCapture.CaptureScreenshot(screenshotPath);

        gameManager.Notification("Screenshot Taken!", Color.green);

        // Wait for the end of the frame to ensure the screenshot is captured
        yield return new WaitForEndOfFrame();

        // Create log entry
        Log log = new Log(userName, screenshotName, inGameTimer.GetTimerString(), timestamp);

        // Log to Excel
        logsManager.LogUserAttempt(log);
    }
}
