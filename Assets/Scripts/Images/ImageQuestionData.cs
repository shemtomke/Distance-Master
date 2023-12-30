using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImageQuestionData
{
    public int id;
    public string title; //image title -> will give you a hint on the specific questions to add to the pictures
    public string image_url;
    public Texture2D texture;
    public Sprite sprite;
    public List<string> questions = new List<string>();
}
[Serializable]
public class QuestionDataWrapper
{
    public List<ImageQuestionData> questions = new List<ImageQuestionData>();
}