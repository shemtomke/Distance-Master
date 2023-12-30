using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public QuestionDataWrapper questionDataWrapper = new QuestionDataWrapper();
    public Text questionTextUI;

    public int currentQuestionIndex = 0;
    public int currentSelectedImage = 0; // When we select from the drop down

    void Start()
    {
        DisplayCurrentQuestion();
    }
    private void Update()
    {
        DisplayCurrentQuestion();
    }

    public void NextQuestion()
    {
        if (currentQuestionIndex < questionDataWrapper.questions.Count)
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
        if (currentQuestionIndex >= 0 && currentQuestionIndex < questionDataWrapper.questions.Count)
        {
            questionTextUI.text = questionDataWrapper.questions[currentSelectedImage].questions[currentQuestionIndex];
        }
    }
}
