using AnotherFileBrowser.Windows;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class FileBrowserUpdate : MonoBehaviour
{
    LoadImages loadImages;
    TimeManager timeManager;
    GameManager gameManager;
    private void Start()
    {
        loadImages = FindObjectOfType<LoadImages>();
        timeManager = FindObjectOfType<TimeManager>();
        gameManager = FindObjectOfType<GameManager>();
    }
    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadImage(path));
        });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                if (uwrTexture != null)
                {
                    var sprite = Sprite.Create(uwrTexture, new Rect(0, 0, uwrTexture.width, uwrTexture.height), Vector2.zero);
                    gameManager.Notification("Successfully Uploaded Image!", Color.green);
                    // Assign the created sprite back to the sprite variable
                    loadImages.selectedImage.sprite = sprite;
                    loadImages.chooseImageUI.SetActive(false);
                    timeManager.customTimerUI.SetActive(true);
                }
            }
        }
    }
}
