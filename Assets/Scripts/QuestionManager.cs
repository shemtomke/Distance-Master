using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public List<ImageQuestionData> imageQuestions = new List<ImageQuestionData>();
    public Text questionTextUI;

    private int currentQuestionIndex = 0;
    public int currentSelectedImage = 0; // When we select from the drop down

    void Start()
    {
        DisplayCurrentQuestion();
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < imageQuestions.Count - 1)
        {
            currentQuestionIndex++;
            DisplayCurrentQuestion();
        }
    }
    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
            DisplayCurrentQuestion();
        }
    }
    private void DisplayCurrentQuestion()
    {
        if (currentQuestionIndex >= 0 && currentQuestionIndex < imageQuestions.Count)
        {
            questionTextUI.text = imageQuestions[currentSelectedImage].questions[currentQuestionIndex];
        }
    }
}
