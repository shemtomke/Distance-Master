using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour
{
    public QuestionDataWrapper questionDataWrapper = new QuestionDataWrapper();
    public GameObject questionSetUI;
    public Text questionTextUI;

    public int currentQuestionIndex = 0;
    public int currentSelectedImage = -1; // When we select an image from the drop down
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
        if (currentQuestionIndex < questionDataWrapper.questions[currentSelectedImage].questions.Count - 1)
        {
            currentQuestionIndex++;
        }
    }
    public void PreviousQuestion()
    {
        if (currentQuestionIndex > 0)
        {
            currentQuestionIndex--;
        }
    }
    private void DisplayCurrentQuestion()
    {
        if (questionDataWrapper.questions == null || currentSelectedImage < 0 || currentSelectedImage >= questionDataWrapper.questions.Count)
            return;

        var selectedImageQuestions = questionDataWrapper.questions[currentSelectedImage];

        if (selectedImageQuestions.questions == null || currentQuestionIndex < 0 || currentQuestionIndex >= selectedImageQuestions.questions.Count)
            return;

        // At this point, both the selected image and current question index are within valid ranges
        questionTextUI.text = selectedImageQuestions.questions[currentQuestionIndex];
    }
}
