using System.Collections.Generic;
using UnityEngine;

public class MapDragBlock : MonoBehaviour
{
    enum Rotation
    {
        up,
        right,
        down,
        left
    } 

    Rotation rotation;
    bool isDragging;
    Vector3 dragOffset;
    Vector3 originalPosition;
    MapGrid grid;

    public List<Vector2> occupied;

    public Camera cam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = FindFirstObjectByType<MapGrid>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            transform.position = SnapToGrid(GetMousePos() + dragOffset);
        }
    }

    private void OnMouseDown()
    {
        isDragging = true;
        dragOffset = transform.position - GetMousePos(); 
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;

    }

    void FindActiveCam()
    {

    }

    Vector3 GetMousePos()
    {
        return new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y);
    }

    Vector2 SnapToGrid(Vector2 pos)
    {
        Vector2Int coords = grid.WorldPosToGridCoord(pos.x, pos.y);
        return grid.GridCoordToWorldPos(coords.x, coords.y, true);
    }
}
