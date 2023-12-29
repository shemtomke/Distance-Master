using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherFileBrowser;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Message;
using System;
using Unity.VisualScripting;

public class FileExplorerImage : MonoBehaviour
{
    public RawImage rawImage;
    public void OpenFileBrowser() //Call this to add an Image
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.png) | *.jpg, *.jpeg, *.jpe, *.jfif, *.png";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            StartCoroutine(LoadFileImage(path));
        });
    }

    IEnumerator LoadFileImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();
            if(uwr.result == UnityWebRequest.Result.ConnectionError|| uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                rawImage.texture = uwrTexture;
            }
        }
    }
}
