using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Implement varying time limits for different difficulty levels (Easy, Medium, Hard), with a customizable option for 'Custom'.
    public bool isStart = false; // after selecting an image
    public bool isGameOver = false;
    public Difficulty currentDifficulty;
    public Text notificationUI;

    LoadImages loadImages;
    TimeManager timeManager;
    MarkerHandler markerHandler;
    UserManager userManager;
    private void Start()
    {
        loadImages = FindObjectOfType<LoadImages>();
        markerHandler = FindObjectOfType<MarkerHandler>();
        timeManager = FindObjectOfType<TimeManager>();
        userManager = FindObjectOfType<UserManager>();

        currentDifficulty = Difficulty.Easy;
    }
    // Take A Screenshot
    public void TakeScreenShot()
    {
        ScreenCapture.CaptureScreenshot("Screenshot - " + userManager.currentUserName + " - " + timeManager.timeTaken + " - "+ markerHandler.distanceTxt.text.ToString() + ".png");
        Notification("Screenshot Taken!", Color.green);
    }
    public void ResetToChooseImage()
    {
        loadImages.itemImageUI.SetActive(false);
        timeManager.customTimerUI.SetActive(false);
        loadImages.imageDropdown.gameObject.SetActive(true);
        loadImages.chooseImageUI.SetActive(true);
        markerHandler.ResetMarkPoints();
        loadImages.imageDropdown.value = -1;
    }
    public void ReloadScene()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex);
    }
    public void Notification(string message, Color color)
    {
        // Set the notification message to the UI text
        notificationUI.gameObject.SetActive(true);
        notificationUI.color = color;
        notificationUI.text = message;
        StartCoroutine(DeactivateAfterDelay(2f));
    }
    IEnumerator DeactivateAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Deactivate the UI or perform any other action here
        notificationUI.gameObject.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
