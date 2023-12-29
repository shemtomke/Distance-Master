using System;
using System.Collections.Generic;

public struct ImageQuestionData
{
    public int id;
    public string title; //image title -> will give you a hint on the specific questions to add to the pictures
    public string imageUrl;
    public List<string> questions;
}
