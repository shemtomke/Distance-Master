using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImageQuestionData
{
    public string title; //image title -> will give you a hint on the specific questions to add to the pictures
    public string image_url;
    public string difficulty;
    public Texture2D texture;
    public Sprite sprite;
    public List<string> questions;

    public ImageQuestionData(string title, string imageUrl, string difficulty, List<string> questions) 
    {
        this.title = title;
        this.image_url = imageUrl;
        this.difficulty = difficulty;
        this.questions = questions;
    }
}
[Serializable]
public class QuestionDataWrapper
{
    public List<ImageQuestionData> questions;
}