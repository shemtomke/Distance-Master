using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class UserManager : MonoBehaviour
{
    string fileName = "";
    public string userJsonUrl = "";

    public List<UserDetails> userDetails = new List<UserDetails>();
    public UserWrapperData userWrapperData;

    [Header("Sign Up")]
    public GameObject signUpUI;
    public Text usernameSignUpField;
    public Text passwordSignUpField;
    public Text confirmPasswordSignUpField;

    [Header("Login")]
    public GameObject loginUI;
    public Text usernameLogInField;
    public Text passwordLogInField;

    private void Awake()
    {
        //LoadUsers();
    }
    private void Start()
    {
        fileName = Application.dataPath + "/userdata.csv";
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("User!");
            WriteCSV();
        }
    }
    // Login & Sign Up
    public void SignUp()
    {
        var username = usernameSignUpField.text.ToString();
        var password = passwordSignUpField.text.ToString();
        var confirmPassword = confirmPasswordSignUpField.text.ToString();
        if(confirmPassword == password)
        {
            if (!UserExists(username))
            {
                User newUser = new User { userName = username, password = password };
                userWrapperData.users.Add(newUser);
                SaveUsers();
                SuccessfulLogin();
            }
        }
        else
        {
            Debug.Log("Password Do not Match!");
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
                Debug.Log("Login successful!");
                SuccessfulLogin();
            }
            else
            {
                Debug.Log("Login unsuccessful!");
                Debug.Log("Username : " + username + ", Password : " + password);
            }
        }
    }
    public void SignOut()
    {

    }
    private bool UserExists(string username)
    {
        return userWrapperData.users.Exists(user => user.userName == username);
    }
    private void SaveUsers()
    {
        string json = JsonUtility.ToJson(userWrapperData.users);
        File.WriteAllText(userJsonUrl, json);
    }
    void SuccessfulLogin()
    {
        loginUI.SetActive(false);
        signUpUI.SetActive(false);
    }

    // Local Data Access: Ensure you can access the gameplay data of all users locally.
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