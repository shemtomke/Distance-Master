using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class UserManager : MonoBehaviour
{
    string fileName = "";
    public string userJsonUrl = "";
    public string currentUserName;
    public Text usernameTxt;

    public List<UserDetails> userDetails = new List<UserDetails>();
    public UserWrapperData userWrapperData;

    public GameObject gameSceneUI;
    

    [Header("Sign Up")]
    public GameObject signUpUI;
    public InputField usernameSignUpField;
    public InputField passwordSignUpField;
    public InputField confirmPasswordSignUpField;

    [Header("Login")]
    public GameObject loginUI;
    public InputField usernameLogInField;
    public InputField passwordLogInField;

    DbManager dbManager;
    GameManager gameManager;
    MarkerHandler markerHandler;
    TimeManager timeManager;
    LoadImages loadImages;

    private void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        gameManager = FindObjectOfType<GameManager>();
        markerHandler = FindObjectOfType<MarkerHandler>();
        timeManager = FindObjectOfType<TimeManager>();
        loadImages = FindObjectOfType<LoadImages>();

        loginUI.SetActive(true);

        fileName = Application.dataPath + "/userdata.csv";
    }
    // Login & Sign Up
    public void SignUp()
    {
        var username = usernameSignUpField.text;
        var password = passwordSignUpField.text;
        var confirmPassword = confirmPasswordSignUpField.text;
        if(confirmPassword == password)
        {
            if (!UserExists(username))
            {
                User newUser = new User(username, password);
                userWrapperData.users.Add(newUser);
                dbManager.CreateUser(newUser);
                SuccessfulLogin();
            }
            else
            {
                gameManager.Notification("User Exists. Try another Username!", Color.red);
            }
        }
        else
        {
            gameManager.Notification("Password Do not Match!", Color.red);
        }
    }
    public void LogIn()
    {
        var username = usernameLogInField.text;
        var password = passwordLogInField.text;
        foreach (User user in userWrapperData.users)
        {
            if (user.userName == username && user.password == password)
            {
                gameManager.Notification("Login successful!", Color.green);
                SuccessfulLogin();
                currentUserName = user.userName;
                usernameTxt.text = "Username : " + currentUserName;
            }
            else
            {
                gameManager.Notification("Login unsuccessful! Incorrect password or username", Color.red);
            }
        }
    }
    
    public void SignOut()
    {
        gameManager.ReloadScene();
    }
    private bool UserExists(string username)
    {
        return userWrapperData.users.Exists(user => user.userName == username);
    }
    void SuccessfulLogin()
    {
        loadImages.PopulateDropdown();
        loginUI.SetActive(false);
        signUpUI.SetActive(false);
        gameSceneUI.SetActive(true);
        loadImages.chooseImageUI.SetActive(true);
    }
    public void SubmitUserDetails()
    {
        timeManager.SaveTime();

        var dist = markerHandler.distanceTxt.text.ToString();
        var time = timeManager.timeTaken;
        var difficulty = gameManager.currentDifficulty.ToString();

        UserDetails newUserDetails = new UserDetails(currentUserName, dist, time, difficulty);
        userDetails.Add(newUserDetails);
        dbManager.CreateUserData(newUserDetails);
        WriteCSV();
        gameManager.TakeScreenShot();
        loadImages.isLoaded = false;

        // Reset Game to Choose Image Page UI
        gameManager.ResetToChooseImage();
    }
    // Local Data Access: Ensure you can access the gameplay data of the current user locally
    public void WriteCSV()
    {
        if(userDetails.Count > 0)
        {
            TextWriter tw = new StreamWriter(fileName, false);
            tw.WriteLine("Username, DistanceMeasured, TimeTaken, DifficultyLevel");
            tw.Close();

            tw = new StreamWriter(fileName, true);
            for (int i = 0; i < userDetails.Count; i++)
            {
                tw.WriteLine(userDetails[i].userName + "," + userDetails[i].distanceMeasured + "," +
                    userDetails[i].timeTaken + "," + userDetails[i].difficultyLevel);
            }
            tw.Close();
        }
    }
}