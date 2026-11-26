using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BGScroller : MonoBehaviour
{
    float curOffset = 0;

    [SerializeField] float tileSpacing;
    [SerializeField] List<ScreenTile> screensTiles;
    [SerializeField] GameObject screenTilePrefab;

    [SerializeField] Transform stationsParent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public void GenerateScreenTiles(List<Vector2Int> tracks)
    {
        ClearScreenTiles();

        for(int i = 0; i < tracks.Count; i++)
        {
            Debug.Log(i + " " + tracks[i]);
            ScreenTile curScreenTile = Instantiate(screenTilePrefab, transform).GetComponent<ScreenTile>();
            curScreenTile.RenderTile(tracks[i]);
            screensTiles.Add(curScreenTile);
        }

        OrganiseScreenTiles();
    }

    public void OrganiseScreenTiles()
    {
        for (int i = 0; i < screensTiles.Count; i++) {
            screensTiles[i].transform.localPosition = new Vector3(tileSpacing * i, 0, 0);
        }
    }

    public void ClearScreenTiles()
    {
        for (int i = 0; i < screensTiles.Count; i++)
        {
            if (screensTiles[i].renderedStation)
            {
                screensTiles[i].renderedStation.transform.SetParent(stationsParent);
            }
            Destroy(screensTiles[i].gameObject);
        }

        screensTiles.Clear();
    }

    public void UpdateBGScroller(int tileNum, float progress)
    {
        float fullLength = screensTiles.Count * tileSpacing;

        transform.localPosition = new Vector3(-(tileNum + progress)*tileSpacing, 0, 0);
        //Debug.Log(tileNum +" "+ progress);
    }
}
