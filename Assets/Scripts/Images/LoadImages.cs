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
    public Dropdown imageDropdown;
    public Image selectedImage; // Use Image component for displaying sprites

    private List<Sprite> sprites = new List<Sprite>();

    public bool isLoaded = false;

    QuestionManager questionManager;
    void Start()
    {
        questionManager = FindObjectOfType<QuestionManager>();

        // Add listener to the dropdown change event
        imageDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnEnable()
    {
        questionManager = FindObjectOfType<QuestionManager>();

        PopulateDropdown();
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

        // Iterate through all difficulty paths
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
    }
    public void LoadImage(int index)
    {
        // Load the selected image into the Image component
        if (index >= 0 && index < sprites.Count)
        {
            Sprite sprite = sprites[index];
            selectedImage.gameObject.SetActive(true);
            selectedImage.sprite = sprite;
            isLoaded = true;

            if (isLoaded)
            {
                imageDropdown.gameObject.SetActive(false);
            }
        }
    }
}
