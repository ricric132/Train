using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class DragManager : MonoBehaviour
{
    public GameObject selectedDragObject;    
    public List<SnappingPoint> snappingpoints = new List<SnappingPoint>(); 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public SnappingPoint CheckSnap()
    {
        float closestDist = Mathf.Infinity;
        SnappingPoint closestPoint = null;

        for(int i = 0; i < snappingpoints.Count; i++)
        {
            if (snappingpoints[i].gameObject.activeInHierarchy == false)
            {
                continue;
            }
            SnappingPoint snapPoint = snappingpoints[i];
            float dist = Vector2.Distance(selectedDragObject.transform.position, snapPoint.gameObject.transform.position);

            if (dist < closestDist && dist < snapPoint.snapRange)
            {
                closestDist = dist;
                closestPoint = snapPoint;
            }
        }

        return closestPoint;
    }

    public void HighlightPlacableSpots(SnappingPoint startingPoint)
    {
        for(int i = 0;i < snappingpoints.Count; i++)
        {
            if (snappingpoints[i].gameObject.activeInHierarchy == false)
            {
                continue;
            }

            if (startingPoint.gameObject.tag == "QueueSpot")
            {
                if (snappingpoints[i].gameObject.tag == "Seat" && snappingpoints[i].occupiedGO == null)
                {
                    snappingpoints[i].ShowIndicator();
                }
            }

            if (startingPoint.gameObject.tag == "Seat")
            {
                if (snappingpoints[i].gameObject.tag == "DepartPoint")
                {
                    snappingpoints[i].ShowIndicator();
                }
            }
        }

    }

    public void UnhighlightAll()
    {
        for (int i = 0; i < snappingpoints.Count; i++)
        {
            snappingpoints[i].StopIndicating();
        }
    }


    public Vector3 GetMousePos()
    {
        return new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }



}
