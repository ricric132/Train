using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    List<Vector2Int> dirs = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GenGridTile[,] GenTrackErosionMap(int width, int height, int steps) // amount of tiles taken out will be steps + 1
    {
        List<Vector2Int> updateCandidates = new List<Vector2Int>();

        GenGridTile[,] grid = new GenGridTile[width, height];
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = new GenGridTile(i, j);
            }
        }
        grid[width/2, height/2].inside = true;
        updateCandidates.Add(new Vector2Int(width / 2, height / 2));

        for (int i = 0; i < steps; i++)
        {
            List<Vector2Int> errodeDirs = new List<Vector2Int>();
            Vector2Int rand = Vector2Int.zero;

            while (errodeDirs.Count == 0 && updateCandidates.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, updateCandidates.Count);
                rand = updateCandidates[index];

                for (int j = 0; j < dirs.Count; j++)
                {
                    Vector2Int checkDir = dirs[j];
                    Vector2Int checkNext = rand + checkDir;

                    if (checkNext.x >= 0 && checkNext.x < width && checkNext.y >= 0 && checkNext.y < height && !grid[checkNext.x, checkNext.y].inside)
                    {
                        errodeDirs.Add(checkDir);
                    }
                }

                if(errodeDirs.Count == 0)
                {
                    updateCandidates.RemoveAt(index);
                }
            }
            
            if(updateCandidates.Count > 0)
            {
                Debug.Log("set");

                Vector2Int errode = rand + errodeDirs[UnityEngine.Random.Range(0, errodeDirs.Count)];
                updateCandidates.Add(errode);
                grid[errode.x, errode.y].inside = true;
            }
            else
            {
                Debug.Log("dead");
            }

        }

        return grid;
    }

}

public class GenGridTile
{
    public int x;
    public int y;
    public bool inside;

    public GenGridTile(int x, int y)
    {
        this.x = x;
        this.y = y;
        inside = false;
    }
}
