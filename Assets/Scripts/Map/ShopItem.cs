using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : MonoBehaviour
{


    Rotation rotation;
    bool isDragging;
    Vector3 dragOffset;
    Vector3 originalPosition;
    public MapGrid map;

    public List<Vector2> occupied;

    public Camera cam;

    public BuildingTemplateSO templateSO; 

    public GameObject itemVisual;
    public GameObject mapVisual;

    public ShopSlot slot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        mapVisual.SetActive(false);
        itemVisual.SetActive(true);
        map = FindFirstObjectByType<MapGrid>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isDragging)
        {
            if (OnMap())
            {
                transform.position = cam.WorldToScreenPoint(SnapToGrid(cam.ScreenToWorldPoint(Input.mousePosition)));
                mapVisual.SetActive(true);
                itemVisual.SetActive(false);
            }
            else
            {
                transform.position = Input.mousePosition + dragOffset;
                mapVisual.SetActive(false);
                itemVisual.SetActive(true);
            }
        }
    }

    public bool OnMap()
    {
        return Input.mousePosition.x < Screen.width * 0.5625f;
    }
    
    public void OnPointerDown()
    {
        isDragging = true;
        dragOffset = transform.position - Input.mousePosition;
    }

    public virtual void OnPointerUp()
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

    Vector2 SnapToGrid(Vector3 pos)
    {
        Vector2Int coords = map.WorldPosToGridCoord(pos.x, pos.y);
        return map.GridCoordToWorldPos(coords.x, coords.y, true);
    }
}
