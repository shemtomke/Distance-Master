using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string userName;
    public string password;

    public User(string  userName, string password)
    {
        this.userName = userName;
        this.password = password;
    }
}
[System.Serializable]
public class UserWrapperData
{
    public List<User> users;
}