using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    string fileName = "";
    public List<UserAccount> userAccounts = new List<UserAccount>();

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
    // Store and display details like the distance measured, time taken, and user descriptions in the account section.
    void AddUserData()
    {

    }
    // Create an entry form to record user account information and gameplay statistics, such as time taken.
    void UserData()
    {

    }
    // Local Data Access: Ensure you can access the gameplay data of all users locally.
    public void WriteCSV()
    {
        if(userAccounts.Count > 0)
        {
            TextWriter tw = new StreamWriter(fileName, false);
            tw.WriteLine("Username, DistanceMeasured, TimeTaken, UserDescription");
            tw.Close();

            tw = new StreamWriter(fileName, true);
            for (int i = 0; i < userAccounts.Count; i++)
            {
                tw.WriteLine(userAccounts[i].username + "," + userAccounts[i].distanceMeasured + "," +
                    userAccounts[i].timeTaken + "," + userAccounts[i].userDescription);
            }
            tw.Close();
        }
    }
}
[System.Serializable]
public class UserAccount
{
    public string username;
    public float distanceMeasured;
    public float timeTaken;
    public string userDescription;

    // Additional properties or methods as needed
}

