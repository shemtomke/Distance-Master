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
    public Toggle confirmPasswordSignUpToggle;
    public Toggle passwordSignUpToggle;

    [Header("Login")]
    public GameObject loginUI;
    public InputField usernameLogInField;
    public InputField passwordLogInField;
    public Toggle passwordLoginToggle;

    private Dictionary<Toggle, InputField> toggleToInputFieldMap;

    DbManager dbManager;
    GameManager gameManager;
    MarkerHandler markerHandler;
    TimeManager timeManager;
    LoadImages loadImages;
    ScreenShotManager screenShotManager;
    private void Start()
    {
        dbManager = FindObjectOfType<DbManager>();
        gameManager = FindObjectOfType<GameManager>();
        markerHandler = FindObjectOfType<MarkerHandler>();
        timeManager = FindObjectOfType<TimeManager>();
        loadImages = FindObjectOfType<LoadImages>();
        screenShotManager = FindObjectOfType<ScreenShotManager>();

        loginUI.SetActive(true);

        fileName = Application.dataPath + "/userdata.csv";

        toggleToInputFieldMap = new Dictionary<Toggle, InputField>
        {
            { confirmPasswordSignUpToggle, confirmPasswordSignUpField },
            { passwordLoginToggle, passwordLogInField },
            { passwordSignUpToggle, passwordSignUpField }
        };

        // Subscribe to the onValueChanged events of the toggles
        foreach (var toggle in toggleToInputFieldMap.Keys)
        {
            toggle.onValueChanged.AddListener((isOn) => ShowPassword(toggle, isOn));
        }
    }
    // Login & Sign Up
    public void SignUp()
    {
        var username = usernameSignUpField.text;
        var password = passwordSignUpField.text;
        var confirmPassword = confirmPasswordSignUpField.text;

        // Check if username or password is null or empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            gameManager.Notification("Username or Password cannot be empty", Color.red);
            return; // Exit the method if username or password is empty
        }

        // Check if passwords match
        if (confirmPassword == password)
        {
            // Check if the user already exists
            if (!UserExists(username))
            {
                User newUser = new User(username, password);
                userWrapperData.users.Add(newUser);
                dbManager.CreateUser(newUser);
                currentUserName = username;
                SuccessfulLogin();
                gameManager.Notification("Sign up successful!", Color.green);
            }
            else
            {
                gameManager.Notification("User already exists. Try another username!", Color.red);
            }
        }
        else
        {
            gameManager.Notification("Passwords do not match!", Color.red);
        }
    }
    public void LogIn()
    {
        var username = usernameLogInField.text;
        var password = passwordLogInField.text;

        // Check if username or password is null or empty
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            gameManager.Notification("Username and password cannot be empty", Color.red);
            return; // Exit the method if username or password is empty
        }

        bool loginSuccessful = false;

        foreach (User user in userWrapperData.users)
        {
            if (user.userName == username && user.password == password)
            {
                gameManager.Notification("Login successful!", Color.green);
                SuccessfulLogin();
                currentUserName = user.userName;
                usernameTxt.text = "Username: " + currentUserName;
                loginSuccessful = true;
                break; // Exit the loop since login is successful
            }
        }

        if (!loginSuccessful)
        {
            gameManager.Notification("Login unsuccessful! Incorrect password or username", Color.red);
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
    private void ShowPassword(Toggle toggle, bool isOn)
    {
        if (toggleToInputFieldMap.TryGetValue(toggle, out InputField inputField))
        {
            inputField.contentType = isOn ? InputField.ContentType.Standard : InputField.ContentType.Password;
            string currentText = inputField.text;
            inputField.text = "";
            inputField.text = currentText;
        }
        else
        {
            Debug.LogError("Toggle not found in the map!");
        }
    }
    void SuccessfulLogin()
    {
        loadImages.PopulateDropdown();
        loginUI.SetActive(false);
        signUpUI.SetActive(false);
        gameSceneUI.SetActive(true);
        loadImages.chooseImageUI.SetActive(true);
    }
    // Submit Button
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
        screenShotManager.TakeScreenShot();

        // Uncomment below to Reset Game to Choose Image Page UI
        //gameManager.ResetToChooseImage();
    }
    // Local Data Access: Ensure you can access the gameplay data of the current user locally (Csv)
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