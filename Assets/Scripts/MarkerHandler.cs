using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;

public class MarkerHandler : MonoBehaviour
{
    public Dropdown measurementsDropdownUI;
    public MeasurementUnits currentUnit = MeasurementUnits.Meter;
    public Vector3 point1, point2;
    public GameObject markerPrefab;
    public Text distanceTxt;
    public LineRenderer lineRenderer; // Reference to the LineRenderer component

    public Button undoBtn, clearBtn;

    List<GameObject> markers = new List<GameObject>();

    Vector2 secondPoint;

    bool isPoint1Marked = false, isPoint2Marked = false;
    bool isSelected = true;

    public float distance;

    public event Action OnMeasurementConditionsMet;

    private Camera mainCamera;

    LoadImages loadImages;
    private void Start()
    {
        loadImages = FindObjectOfType<LoadImages>();

        mainCamera = Camera.main;

        OnMeasurementConditionsMet += Measure;

        PopulateDropdown();

        int defaultIndex = Array.IndexOf(Enum.GetNames(typeof(MeasurementUnits)), MeasurementUnits.Meter.ToString());
        measurementsDropdownUI.value = defaultIndex;

        OnDropdownValueChanged(defaultIndex);
        measurementsDropdownUI.onValueChanged.AddListener(OnDropdownValueChanged);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && loadImages.isLoaded)
        {
            if (isSelected)
            {
                MarkPoint1();
            }
            else
            {
                MarkPoint2();
            }
            // Toggle the marking mode
            isSelected = !isSelected;
        }

        LineToolBar();
    }
    void MarkPoint1()
    {
        if (!isPoint1Marked && !isPoint2Marked)
        {
            point1 = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            GameObject obj = Instantiate(markerPrefab, point1, Quaternion.identity);
            markers.Add(obj);
            isPoint1Marked = true;
            Debug.Log("Marked Point 1");
        }
    }
    void MarkPoint2()
    {
        if (isPoint1Marked && !isPoint2Marked)
        {
            point2 = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            secondPoint = point2;
            GameObject obj = Instantiate(markerPrefab, point2, Quaternion.identity);
            markers.Add(obj);
            isPoint2Marked = true;
            Debug.Log("Marked Point 2");
            OnMeasurementConditionsMet?.Invoke();
        }
    }
    public void ResetMarkPoints()
    {
        DestroyMarkers();
        lineRenderer.gameObject.SetActive(false);
        isPoint1Marked = false;
        isPoint2Marked = false;
        isSelected = true;
    }
    public void Undo()
    {
        if(isPoint1Marked && !isPoint2Marked)
        {
            ResetMarkPoints();
        }
        else if(isPoint1Marked && isPoint2Marked)
        {
            Destroy(markers[markers.Count - 1]); //last
            lineRenderer.gameObject.SetActive(false);
            isPoint2Marked = false;
            isSelected = false;
        }
    }
    public void Clear()
    {
        ResetMarkPoints();
    }
    void LineToolBar()
    {
        if((isPoint1Marked && isPoint2Marked) || (isPoint1Marked || isPoint2Marked))
        {
            undoBtn.interactable = true;
            clearBtn.interactable = true;
        }
        else
        {
            undoBtn.interactable = false;
            clearBtn.interactable = false;
        }
    }
    public float MeasureDistance(Vector3 p1, Vector3 p2)
    {
        return Vector3.Distance(p1, p2);
    }

    public void Measure()
    {
        if (isPoint1Marked && isPoint2Marked)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, point1);
            lineRenderer.SetPosition(1, point2);

            // Calculate the average position of point1 and point2
            Vector3 avgPosition = (point1 + point2) / 2f;

            distanceTxt.gameObject.SetActive(true);
            // Set the position of the distance text above the line
            //distanceTxt.transform.position = avgPosition + new Vector3(0f, 0.5f, 0f); // Adjust the '0.5f' as needed

            distance = MeasureDistance(point1, point2);
            distanceTxt.text = distance + " " + currentUnit.ToString();
        }
        else
        {
            distanceTxt.gameObject.SetActive(false);
        }
    }
    public void DestroyMarkers()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            Destroy(markers[i]);
        }
    }
    void PopulateDropdown()
    {
        // Get all enum values
        List<string> enumNames = new List<string>(Enum.GetNames(typeof(MeasurementUnits)));

        // Clear existing options
        measurementsDropdownUI.ClearOptions();

        // Populate dropdown with enum values
        measurementsDropdownUI.AddOptions(enumNames);
    }
    public void OnDropdownValueChanged(int index)
    {
        // Handle the selected enum value
        MeasurementUnits selectedUnit = (MeasurementUnits)Enum.Parse(typeof(MeasurementUnits), measurementsDropdownUI.options[index].text);

        // Get the conversion factor from the current unit to the selected unit
        float conversionFactor = GetConversionFactor(currentUnit, selectedUnit);

        // Update the current unit before modifying the distance
        currentUnit = selectedUnit;

        // Apply the conversion to the distance
        var updatedDist = distance * conversionFactor;
        distance = updatedDist;
        // Update the UI text or perform any other necessary actions
        distanceTxt.text = updatedDist + " " + currentUnit.ToString();
        Debug.Log("Updated distance: " + updatedDist);
    }
    // cm, mm, km - changing measurement
    private float GetConversionFactor(MeasurementUnits fromUnit, MeasurementUnits toUnit)
    {
        switch (fromUnit)
        {
            case MeasurementUnits.Centimeter:
                switch (toUnit)
                {
                    case MeasurementUnits.Meter:
                        return 0.01f;
                    case MeasurementUnits.Kilometre:
                        return 1e-5f;
                    case MeasurementUnits.Millimetre:
                        return 10f;
                    default:
                        return 1f;
                }
            case MeasurementUnits.Meter:
                switch (toUnit)
                {
                    case MeasurementUnits.Centimeter:
                        return 100f;
                    case MeasurementUnits.Kilometre:
                        return 0.001f;
                    case MeasurementUnits.Millimetre:
                        return 1000f;
                    default:
                        return 1f;
                }
            case MeasurementUnits.Kilometre:
                switch (toUnit)
                {
                    case MeasurementUnits.Centimeter:
                        return 1e5f;
                    case MeasurementUnits.Meter:
                        return 1000f;
                    case MeasurementUnits.Millimetre:
                        return 1e6f;
                    default:
                        return 1f;
                }
            case MeasurementUnits.Millimetre:
                switch (toUnit)
                {
                    case MeasurementUnits.Centimeter:
                        return 0.1f;
                    case MeasurementUnits.Meter:
                        return 0.001f;
                    case MeasurementUnits.Kilometre:
                        return 1e-6f;
                    default:
                        return 1f;
                }
            default:
                return 1f; // No conversion needed if units are the same
        }
    }
}
