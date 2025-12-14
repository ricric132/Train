using UnityEngine;

public class MapTile : MonoBehaviour
{
    public int x;
    public int y;
    public bool inside;
    public bool track;
    public bool empty;
    public bool loopStart;
    public GameObject go;

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
        if (go && go.tag == "Station")
        {
            Debug.Log(go);
            return go.GetComponent<StationTile>().station;
        }

        return null;
    }

    public string GetTag()
    {
        if (go)
        {
            return go.tag;
        }

        return null;
    }
}


