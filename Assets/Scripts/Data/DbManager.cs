using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

public class DbManager : MonoBehaviour
{
    int id = 0, userDetailsId;
    string userID;

    DatabaseReference dbReference;
    UserManager userManager;
    QuestionManager questionManager;
    private void Awake()
    {
        userManager = FindObjectOfType<UserManager>();
        questionManager = FindObjectOfType<QuestionManager>();

        id = PlayerPrefs.GetInt("LastUserId", 0);
        userDetailsId = PlayerPrefs.GetInt("LastUserDetailsId", 0);

        userID = SystemInfo.deviceUniqueIdentifier;

        // Allow Offline Realtime Database
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(true);
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;

        FetchAndUpdateUserWrapperData(); //Logins & Sign Up
        FetchAndUpdateImagesData(); //Images
        FetchAndUpdateUserData(); // User Statistics
    }

    #region Images
    public IEnumerator GetImageData(Action<List<ImageQuestionData>> onCallback)
    {
        var imagesData = dbReference.Child("images").GetValueAsync();
        yield return new WaitUntil(() => imagesData.IsCompleted);

        if (imagesData.Exception != null)
        {
            Debug.LogError($"Error fetching all users info: {imagesData.Exception}");
            yield break; // Exit the coroutine on error
        }

        DataSnapshot imagesSnapshot = imagesData.Result;

        List<ImageQuestionData> imagesList = new List<ImageQuestionData>();

        foreach (var id in imagesSnapshot.Children)
        {
            string imageId = id.Key;

            string imageUrl = id.Child("image_url")?.Value?.ToString();
            string title = id.Child("title")?.Value?.ToString();
            string difficulty = id.Child("difficulty")?.Value?.ToString();

            List<string> questionsList = new List<string>();

            // Assuming "questions" is another list of child nodes
            foreach (var question in id.Child("questions").Children)
            {
                string questionValue = question.Value?.ToString();
                if (questionValue != null)
                {
                    questionsList.Add(questionValue);
                }
            }

            imagesList.Add(new ImageQuestionData(title, imageUrl, difficulty, questionsList));
        }

        for (int i = 0; i < imagesList.Count; i++)
        {
            yield return StartCoroutine(LoadImage(imagesList, imagesList[i].image_url, i));
        }

        onCallback.Invoke(imagesList);
    }
    public void FetchAndUpdateImagesData()
    {
        StartCoroutine(GetImageData((imagesList) =>
        {
            // Update userWrapperData or take any other actions with the list of users
            questionManager.questionDataWrapper.questions.AddRange(imagesList);
        }));
    }
    IEnumerator LoadImage(List<ImageQuestionData> imagesList, string imageUrl, int questionIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            imagesList[questionIndex].texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            ConvertTexturesToSprites(imagesList);
        }
    }
    void ConvertTexturesToSprites(List<ImageQuestionData> imagesList)
    {
        for (int i = 0; i < imagesList.Count; i++)
        {
            var texture = imagesList[i].texture;

            if (texture != null)
            {
                var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

                // Assign the created sprite back to the sprite variable
                imagesList[i].sprite = sprite;
            }
        }
    }
    #endregion

    #region User Login/Sign Up
    public void CreateUser(User newUser)
    {
        string json = JsonUtility.ToJson(newUser);
        // Check if dbReference is not null before using it
        if (dbReference != null)
        {
            id++;
            PlayerPrefs.SetInt("LastUserId", id);
            PlayerPrefs.Save();
            dbReference.Child("users").Child(userID).Child(id.ToString()).SetRawJsonValueAsync(json);
        }
        else
        {
            Debug.LogError("Database reference is null. Make sure Start method is called.");
        }
    }
    public IEnumerator GetAllUserInfos(Action<List<User>> onCallback)
    {
        var usersData = dbReference.Child("users").GetValueAsync();

        yield return new WaitUntil(() => usersData.IsCompleted);

        if (usersData.Exception != null)
        {
            Debug.LogError($"Error fetching all users info: {usersData.Exception}");
            yield break; // Exit the coroutine on error
        }

        DataSnapshot usersSnapshot = usersData.Result;

        List<User> userList = new List<User>();

        foreach (var userChild in usersSnapshot.Children)
        {
            string userId = userChild.Key;
            foreach (var id in userChild.Children)
            {
                string _id = id.Key;
                string userName = id.Child("userName").Value.ToString();
                string password = id.Child("password").Value.ToString();

                userList.Add(new User(userName, password));
            }
        }

        onCallback.Invoke(userList);
    }

    // Example of how to use the coroutine and update userWrapperData
    public void FetchAndUpdateUserWrapperData()
    {
        StartCoroutine(GetAllUserInfos((userList) =>
        {
            // Update userWrapperData or take any other actions with the list of users
            userManager.userWrapperData.users.AddRange(userList);
        }));
    }
    #endregion

    #region User Statistics Data
    public void CreateUserData(UserDetails newUserDetails)
    {
        string json = JsonUtility.ToJson(newUserDetails);
        // Check if dbReference is not null before using it
        if (dbReference != null)
        {
            userDetailsId++;
            PlayerPrefs.SetInt("LastUserDetailsId", userDetailsId);
            PlayerPrefs.Save();
            dbReference.Child("userDetails").Child(userID).Child(userDetailsId.ToString()).SetRawJsonValueAsync(json);
        }
        else
        {
            Debug.LogError("Database reference is null. Make sure Start method is called.");
        }
    }
    public IEnumerator GetUsereData(Action<List<UserDetails>> onCallback)
    {
        var userData = dbReference.Child("userDetails").GetValueAsync();
        yield return new WaitUntil(() => userData.IsCompleted);

        if (userData.Exception != null)
        {
            Debug.LogError($"Error fetching all users info: {userData.Exception}");
            yield break; // Exit the coroutine on error
        }

        DataSnapshot userDataSnapshot = userData.Result;

        List<UserDetails> userDetailsList = new List<UserDetails>();

        foreach (var userDataChild in userDataSnapshot.Children)
        {
            string userId = userDataChild.Key;
            foreach (var id in userDataChild.Children)
            {
                string _id = id.Key;
                string userName = id.Child("userName").Value.ToString();
                string dist = id.Child("distanceMeasured").Value.ToString();
                string timeTaken = id.Child("timeTaken").Value.ToString();
                string difficulty = id.Child("difficultyLevel").Value.ToString();

                userDetailsList.Add(new UserDetails(userName, dist, timeTaken, difficulty));
            }
        }

        onCallback.Invoke(userDetailsList);
    }
    public void FetchAndUpdateUserData()
    {
        StartCoroutine(GetUsereData((userDetailsList) =>
        {
            userManager.userDetails.AddRange(userDetailsList);
        }));
    }
    #endregion
}
