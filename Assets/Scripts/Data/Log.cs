using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Log
{
    public string userName;
    public string screenshotImageName;
    public string timeTaken;
    public string dateTime;

    public Log(string userName, string screenshotImageName, string timeTaken, string dateTime)
    {
        this.userName = userName;
        this.screenshotImageName = screenshotImageName;
        this.timeTaken = timeTaken;
        this.dateTime = dateTime;
    }
}
