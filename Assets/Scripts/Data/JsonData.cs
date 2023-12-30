using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;

public class JsonData : MonoBehaviour
{
    public string jsonUrl = "";
    string downloadUrl = "https://drive.google.com/uc?export=download&id=";

    LoadImages loadImages;
    QuestionManager questionManager;
    UserManager userManager;

    private void Start()
    {
        loadImages = FindObjectOfType<LoadImages>();
        questionManager = FindObjectOfType<QuestionManager>();
        userManager = FindObjectOfType<UserManager>();

        StartCoroutine(ReadImageJsonData(jsonUrl));
        StartCoroutine(ReadUserJsonData(userManager.userJsonUrl));
    }

    #region Images
    string CutImageUrl(string imageUrl)
    {
        // Find the index of "file/d/" in the URL
        int startIndex = imageUrl.IndexOf("file/d/") + "file/d/".Length;

        // Find the index of "/view" in the URL
        int endIndex = imageUrl.IndexOf("/view");

        // Extract the substring between startIndex and endIndex
        string extractedString = imageUrl.Substring(startIndex, endIndex - startIndex);

        return downloadUrl + extractedString;
    }
    public IEnumerator ReadImageJsonData(string jsonUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse JSON response
            questionManager.questionDataWrapper = JsonUtility.FromJson<QuestionDataWrapper>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);

            for (int i = 0; i < questionManager.questionDataWrapper.questions.Count; i++)
            {
                string modifiedImageUrl = CutImageUrl(questionManager.questionDataWrapper.questions[i].image_url);
                yield return StartCoroutine(GetImage(modifiedImageUrl, i));

                ConvertTexturesToSprites();
            }
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
    IEnumerator GetImage(string imageUrl, int questionIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            questionManager.questionDataWrapper.questions[questionIndex].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
    void ConvertTexturesToSprites()
    {
        for (int i = 0; i < questionManager.questionDataWrapper.questions.Count; i++)
        {
            var texture = questionManager.questionDataWrapper.questions[i].texture;

            if (texture != null)
            {
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Assign the created sprite back to the sprite variable
                questionManager.questionDataWrapper.questions[i].sprite = sprite;
                Debug.Log("Load Sprite: " + questionManager.questionDataWrapper.questions[i].sprite.name);
            }
        }
    }
    #endregion

    #region User
    public IEnumerator ReadUserJsonData(string jsonUrl)
    {
        UnityWebRequest request = UnityWebRequest.Get(jsonUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Parse JSON response
            userManager.userWrapperData = JsonUtility.FromJson<UserWrapperData>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
    #endregion
}
