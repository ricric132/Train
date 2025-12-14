using UnityEngine;

public class Building : MonoBehaviour
{
    public int stationEffectRange = 1;


    public MapGrid map;
    public Vector2Int coords;

    public virtual void Awake()
    {

    }

    public virtual void Setup()
    {
        //make more robust later

    }

    public virtual void Remove()
    {
        
    }
}
