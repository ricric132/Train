using UnityEngine;

public class MapTile
{
    public int x;
    public int y;
    public bool inside;
    public bool track;
    public bool empty;
    public bool loopStart;

    private GameObject builtObject;
    public GameObject BuiltObject
    {
        get
        {
            return builtObject;
        }
        set
        {
            if (builtObject)
            {
                Object.Destroy(builtObject);
                Debug.Log("builtObject Overwritten");
            }
            builtObject = value;
        }
    }

    private TerrainType terrain;


    private GameObject terrainObject;
    public GameObject TerrainObject
    {
        get
        {
            return terrainObject;
        }
        set
        {
            if (terrainObject)
            {
                Object.Destroy(terrainObject);
                Debug.Log("terrainObject Overwritten");
            }
            terrainObject = value;
        }
    }

    public GameObject trackTile;
    public GameObject TrackTile
    {
        get
        {
            return trackTile;
        }
        set
        {
            if (trackTile)
            {
                Object.Destroy(trackTile);
                Debug.Log("trackObject Overwritten");
            }
            trackTile = value;
        }
    }

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


