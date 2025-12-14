using UnityEngine;

public class BuildRule : MonoBehaviour
{
    public virtual bool Check(MapTile[,] grid, Vector2Int coords)
    {
        return true;
    }
}
