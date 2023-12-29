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
    public string downloadUrl = "https://drive.google.com/uc?export=download&id="; // download url + image cut url
    public string imagesUrlPath = "";
    public string jsonUrlPath;

    // Load all images from three paths
    [Header("Paths")]
    public string easyPath;
    public string mediumPath;
    public string hardPath;
    public string customPath;

    [Space(20)]

    public Image imageContent;

    public Dropdown imageDropdown;
    public Image selectedImage; // Use Image component for displaying sprites

    private List<Sprite> sprites = new List<Sprite>();

    public bool isLoaded = false;

    QuestionManager questionManager;
    void Start()
    {
        // Set up the dropdown options
        PopulateDropdown();

        // Add listener to the dropdown change event
        imageDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    void PopulateDropdown()
    {
        // Clear existing options in the dropdown
        imageDropdown.ClearOptions();

        // Iterate through all difficulty paths
        foreach (string path in GetImagePaths())
        {
            // Get all image files in the folder (with any supported image format)
            string[] imageFiles = Directory.GetFiles(path, "*.*")
                .Where(file => file.ToLower().EndsWith("png") || file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("jpeg") || file.ToLower().EndsWith("gif"))
                .ToArray();

            // Populate the dropdown with image file names
            foreach (string imagePath in imageFiles)
            {
                string imageName = Path.GetFileNameWithoutExtension(imagePath);
                Sprite sprite = LoadSprite(imagePath);
                sprites.Add(sprite);
                imageDropdown.options.Add(new Dropdown.OptionData(sprite));
            }
        }

        // Refresh the dropdown
        imageDropdown.RefreshShownValue();
    }

    List<string> GetImagePaths()
    {
        // Create a list to store all paths
        List<string> allPaths = new List<string>();

        // Add each difficulty path to the list
        allPaths.Add(easyPath);
        allPaths.Add(mediumPath);
        allPaths.Add(hardPath);
        allPaths.Add(customPath);

        // Remove any empty or null paths
        allPaths.RemoveAll(string.IsNullOrEmpty);

        return allPaths;
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

    Sprite LoadSprite(string path)
    {
        // Load the sprite from the specified path
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // LoadImage automatically resizes the texture

        // Create a sprite from the loaded texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

        return sprite;
    }
    // Load from Google drive
    // cut the string from where
    string CutImageUrl(string imageUrl)
    {
        // Find the index of "file/d/" in the URL
        int startIndex = imageUrl.IndexOf("file/d/") + "file/d/".Length;

        // Find the index of "/view" in the URL
        int endIndex = imageUrl.IndexOf("/view");

        // Extract the substring between startIndex and endIndex
        string extractedString = imageUrl.Substring(startIndex, endIndex - startIndex);

        return extractedString;
    }
    public void FetchImagesUrl(string pathUrl)
    {
        List<string> images = new List<string>();

        // get all total amount of files in the path file of the google drive

    }
    // add the pictures to the json file 
    IEnumerator GetData(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // error ...

        }
        else
        {
            // success...
            ImageQuestionData data = JsonUtility.FromJson<ImageQuestionData>(request.downloadHandler.text);

            // print data in UI
            //uiNameText.text = data.Name;

            // Load image:
            StartCoroutine(GetImage(data.imageUrl));
        }

        // Clean up any resources it is using.
        request.Dispose();
    }

    IEnumerator GetImage(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            // error ...

        }
        else
        {
            //success...
            //uiRawImage.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }

        // Clean up any resources it is using.
        request.Dispose();
    }
}
