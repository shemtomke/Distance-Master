using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LoadImages : MonoBehaviour
{
    public GameObject itemImageUI;
    public GameObject chooseImageUI;
    public Dropdown imageDropdown;
    public Image selectedImage; // Use Image component for displaying sprites

    private List<Sprite> sprites = new List<Sprite>();

    public bool isLoaded = false;

    QuestionManager questionManager;
    GameManager gameManager;
    TimeManager timeManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        questionManager = FindObjectOfType<QuestionManager>();
        timeManager = FindObjectOfType<TimeManager>();

        PopulateDropdown();

        // Add listener to the dropdown change event
        imageDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    public void PopulateDropdown()
    {
        // Check if imageDropdown is null
        if (imageDropdown == null)
        {
            Debug.LogError("imageDropdown is not assigned.");
            return;
        }

        // Clear existing options in the dropdown
        imageDropdown.ClearOptions();

        // Check if questionManager is null
        if (questionManager == null)
        {
            Debug.LogError("questionManager  is not assigned.");
            return;
        }

        if (questionManager.questionDataWrapper == null)
        {
            Debug.LogError("questionManager is not assigned.");
            return;
        }

        for (int i = 0; i < questionManager.questionDataWrapper.questions.Count; i++)
        {
            var img = questionManager.questionDataWrapper.questions[i];
            Sprite sprite = img.sprite;

            // Check if sprite is null
            if (sprite != null)
            {
                sprites.Add(sprite);
                imageDropdown.options.Add(new Dropdown.OptionData(sprite));
            }
            else
            {
                Debug.LogWarning($"Sprite for question {i} is null.");
            }
        }

        // Refresh the dropdown
        imageDropdown.RefreshShownValue();
    }

    void OnDropdownValueChanged(int index)
    {
        // Load the selected image when the dropdown value changes
        LoadImage(index);
        questionManager.currentSelectedImage = index;
        var difficultyEnumString = questionManager.questionDataWrapper.questions[index].difficulty;

        if (Enum.TryParse(difficultyEnumString, out Difficulty parsedDifficulty))
        {
            // Parsing successful, assign the parsed enum value to gameManager.currentDifficulty
            gameManager.currentDifficulty = parsedDifficulty; 
        }
        else
        {
            // Parsing failed, handle the error or provide a default value
            Debug.LogError("Failed to parse difficulty enum from string: " + difficultyEnumString);
        }

        timeManager.GetCurrentTime();
        itemImageUI.SetActive(true);
        questionManager.questionSetUI.SetActive(true);
        chooseImageUI.SetActive(false);
        gameManager.isStart = true;
    }
    public void LoadImage(int index)
    {
        // Load the selected image into the Image component
        if (index >= 0 && index < sprites.Count)
        {
            Sprite sprite = sprites[index];
            selectedImage.sprite = sprite;
            isLoaded = true;

            if (isLoaded)
            {
                imageDropdown.gameObject.SetActive(false);
            }
            else
            {
                imageDropdown.gameObject.SetActive(true);
            }
        }
    }
}
