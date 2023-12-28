using System;
using System.Collections.Generic;

[Serializable]
public class ImageQuestionData
{
    public int id;
    public string imageUrl;
    public List<Questions> questions = new List<Questions>();
}
[Serializable]
public class Questions
{
    public string question;
}
