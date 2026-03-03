using System.Collections.Generic;
using UnityEngine;

public class CantBuildOn : BuildRule
{
    public List<string> exclude;

    public override bool Check(MapTile[,] grid, Vector2Int coords)
    {
        for (int i = 0; i < exclude.Count; i++)
        {
            if (grid[coords.x, coords.y].GetTag() == exclude[i])
            {
                return false;
            }
        }
        return true;
    }
}
