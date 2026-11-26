using UnityEngine;

public class MapTile
{
    public int x;
    public int y;
    public bool inside;
    public bool track;
    public bool empty;
    public bool loopStart;

    public GameObject builtObject;
    public TerrainType terrain;
    public GameObject terrainObject;
    public GameObject trackTile;

    public MapTile(int _x, int _y)
    {
        x = _x;
        y = _y;
        inside = false;
        track = false;
        loopStart = false;
    }

    public Station GetStation()
    {
        if (builtObject && builtObject.tag == "Station")
        {
           // Debug.Log(go);
            return builtObject.GetComponent<StationTile>().station;
        }

        return null;
    }

    public string GetTag()
    {
        if (builtObject)
        {
            return builtObject.tag;
        }

        return null;
    }
}

public enum TerrainType
{
    none, 
    grass,
    mountain,
    water
}

