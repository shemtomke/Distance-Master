using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerHandler : MonoBehaviour
{
    public Vector2 point1, point2;
    bool isPoint1Marked = false, isPoint2Marked = false;
    private void Update()
    {
        MarkPoint1();
        MarkPoint2();
        Measure();
    }
    void MarkPoint1()
    {
        if(!isPoint1Marked && !isPoint2Marked)
        {
            if(Input.GetMouseButtonDown(0))
            {
                point1 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                isPoint1Marked = true;
            }
        }
    }
    void MarkPoint2()
    {
        if (isPoint1Marked && !isPoint2Marked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                point2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                isPoint2Marked = true;
            }
        }
    }
    //Measure a Distance between two Points
    public float MeasureDistance(Vector2 point1, Vector2 point2)
    {
        return Vector2.Distance(point1, point2);
    }
    public void Measure()
    {
        if(isPoint1Marked && isPoint2Marked)
        {
            MeasureDistance(point1, point2);
            Debug.Log(MeasureDistance(point1, point2));
        }
    }
}
