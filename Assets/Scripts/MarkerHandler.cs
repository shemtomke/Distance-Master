using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerHandler : MonoBehaviour
{
    public Vector3 point1, point2;
    public GameObject markerPrefab;
    public Text distanceTxt;
    public LineRenderer lineRenderer; // Reference to the LineRenderer component

    List<GameObject> markers = new List<GameObject>();
    bool isPoint1Marked = false, isPoint2Marked = false;
    bool isSelected = true;

    private Camera mainCamera;

    LoadImages loadImages;
    private void Start()
    {
        loadImages = FindObjectOfType<LoadImages>();

        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && loadImages.isLoaded)
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

        Measure();
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
            GameObject obj = Instantiate(markerPrefab, point2, Quaternion.identity);
            markers.Add(obj);
            isPoint2Marked = true;
            Debug.Log("Marked Point 2");
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

            float distance = MeasureDistance(point1, point2);
            distanceTxt.text = distance + " Metres";
        }
    }
    public void DestroyMarkers()
    {
        for (int i = 0; i < markers.Count; i++)
        {
            Destroy(markers[i]);
        }
    }
}