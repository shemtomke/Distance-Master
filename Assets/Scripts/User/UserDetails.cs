using System;

[Serializable]
public class UserDetails
{
    public string userName;
    public string distanceMeasured;
    public string timeTaken;
    public string difficultyLevel;

    public UserDetails(string userName, string distanceMeasured, string timeTaken, string difficultyLevel)
    {
        this.userName = userName;
        this.distanceMeasured = distanceMeasured;
        this.timeTaken = timeTaken;
        this.difficultyLevel = difficultyLevel;
    }
}
