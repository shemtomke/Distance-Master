using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public string password;
}
[System.Serializable]
public class UserWrapperData
{
    public List<User> users = new List<User>();
}