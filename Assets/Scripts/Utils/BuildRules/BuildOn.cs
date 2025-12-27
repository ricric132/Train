using System.Collections.Generic;
using UnityEngine;

public class BuildOn : BuildRule
{
    public List<string> targets;

    public override bool Check(MapTile[,] grid, Vector2Int coords)
    {
        for(int i = 0; i < targets.Count; i++)
        {
            if(grid[coords.x, coords.y].GetTag() == targets[i])
            {
                return true;
            }
        }
        return false;
    }
}
