using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class MapGrid : MonoBehaviour
{
    const int width = 20;
    const int height = 5;

    int stationDistance = 3;

    const int cornerZoneMax = 6;
    const int cornerZoneMin = 3;

    const int borderSize = 1;

    //map info
    public MapTile[,] grid = new MapTile[width, height];
    float tileSize = 1;
    [SerializeField] Transform origin;
    int generationSteps = 8;
    int startingStations = 3;

    public GameObject mapTilePrefabIn;
    public GameObject mapTilePrefabOut;
    public GameObject startTilePrefab;

    public GameObject vTrackTilePrefab;
    public GameObject hTrackTilePrefab;
    public GameObject tlTrackTilePrefab;
    public GameObject trTrackTilePrefab;
    public GameObject blTrackTilePrefab;
    public GameObject brTrackTilePrefab;


    List<Vector2Int> directions = new List<Vector2Int>() { new Vector2Int(1, 0), new Vector2Int(-1, 0), new Vector2Int(0, 1), new Vector2Int(0, -1) };

    public List<Vector2Int> trainTrack = new List<Vector2Int>();

    StationPath stationPath;

    [SerializeField] Transform visualParent;
    [SerializeField] Transform buildingParent;
    [SerializeField] Transform terrainParent;


    [SerializeField] List<GameObject> terrainPrefabs;


    void Start()
    {
        stationPath = FindFirstObjectByType<StationPath>();
        SetUpGrid();
    }

    void SetUpGrid()
    {
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                grid[i, j] = new MapTile(i, j);
            }
        }

        GenerateLandscape();
        GeneratePath();
        PlaceStartingTile();
        

        GameManager.Instance.bgScroller.GenerateScreenTiles(trainTrack);


    }

    public void Build(Vector2Int coords, Rotation rotation, BuildingTemplateSO template)
    {
        GameObject build = Instantiate(template.mapPrefab, GridCoordToWorldPos(coords.x, coords.y), Quaternion.identity);
        build.transform.parent = buildingParent;
        

        for(int i = 0; i < template.occupiedSpaces.Count; i++)
        {
            Vector2Int offset = template.occupiedSpaces[i];
            grid[coords.x + offset.x, coords.y + offset.y].BuiltObject = build;
            grid[coords.x + offset.x, coords.y + offset.y].empty = false;
            Debug.Log("Built " + grid[coords.x + offset.x, coords.y + offset.y].BuiltObject);
        }

        if(template.GetTag() == "Station")
        {
            build.GetComponent<StationTile>().station = stationPath.AddStation(template);
            build.GetComponent<StationTile>().coords = coords;
            Debug.Log("Station built" + " at " + coords.x + " "  + coords.y);
        }


        if (template.GetTag() == "Building")
        {
            build.GetComponent<Building>().coords = coords;
            build.GetComponent<Building>().map = this;
            build.GetComponent<Building>().Setup();
        }
    }

   

    public Vector2 GridCoordToWorldPos(int x, int y, bool centred = false)
    {
        if(centred) { 
            return new Vector2(origin.position.x + x * tileSize, origin.position.y + y * tileSize) + new Vector2(tileSize/2, tileSize/2);
        }
        else
        {
            return new Vector2(origin.position.x + x * tileSize, origin.position.y + y * tileSize);
        }
    }

    public Vector2Int WorldPosToGridCoord(float x, float y)
    {
        Vector2 relativeToOrigin = new Vector2(x - origin.position.x, y - origin.position.y);
        return new Vector2Int((int)(relativeToOrigin.x/tileSize), (int)(relativeToOrigin.y/tileSize));
    }

    void GenerateLandscape()
    {
        //heightmap based
        Vector2 origin = new Vector2(Random.value, Random.value);
        float stepSize = 0.32f;
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                float value = Mathf.PerlinNoise(i * stepSize + origin.x, j * stepSize + origin.y);

                if(value < 0.3)
                {
                    SetTerrain(new Vector2Int(i, j), TerrainType.water);
                }
                else if(value < 0.8)
                {
                    SetTerrain(new Vector2Int(i, j), TerrainType.grass);
                }
                else
                {
                    SetTerrain(new Vector2Int(i, j), TerrainType.mountain);
                }
            }
        }
    }

    void SetTerrain(Vector2Int coords, TerrainType terrainType)
    {
        GameObject terrainBG = Instantiate(terrainPrefabs[(int)terrainType], GridCoordToWorldPos(coords.x, coords.y), Quaternion.identity);
        terrainBG.transform.SetParent(terrainParent);
        grid[coords.x, coords.y].TerrainObject = terrainBG;

    }


    void GeneratePath()
    {
        if (RunBuffData.selectedConductor == null)
        { 
            Vector2Int prevCoords = new Vector2Int(0, 0);
            Vector2Int coords = new Vector2Int(0, 0);

            coords.y = Random.Range(0, height);
            BuildingTemplateSO randStation = GameManager.Instance.stationPath.GetRandomStation();
            prevCoords = coords;


            Build(coords, Rotation.right, randStation);

            for (int i = 1; i < 3; i++)
            {
                coords.x = 1 + i * stationDistance + Random.Range(0, stationDistance);
                coords.y = Random.Range(0, height);

                randStation = GameManager.Instance.stationPath.GetRandomStation();

                Build(coords, Rotation.right, randStation);

                List<Vector2Int> path = ConnectPoints(prevCoords, coords - new Vector2Int(1, 0));
                path.Add(coords);
                BuildTrackPath(path);

                prevCoords = coords;
            }

        }
        else
        {
            stationDistance = 20 / RunBuffData.selectedConductor.maxStations;
            for (int i = 0; i < RunBuffData.selectedConductor.startingBuildings.Count; i++)
            {
            }
        }
    }

    List<Vector2Int> ConnectPoints(Vector2Int start, Vector2Int end)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        int direction = 0;
        Vector2Int cur = start;
        path.Add(cur); 

        while (cur != end) 
        { 
            if(direction%2 == 0 && cur.x != end.x)
            {
                int step = Random.Range(1, end.x - cur.x + 1);
                for(int i = 0; i < step; i++)
                {
                    cur.x += 1;
                    path.Add(cur);
                }

            }
            else if (cur.y != end.y)
            {
                int step = Random.Range(1, Mathf.Abs(end.y - cur.y) + 1);
                for (int i = 0; i < step; i++)
                {
                    cur.y += (end.y - cur.y)/Mathf.Max(Mathf.Abs(end.y - cur.y), 1);
                    path.Add(cur);
                }
            }

            direction++;
        }

        return path;
    }

    void BuildTrackPath(List<Vector2Int> path)
    {

        Vector2Int enterDir;
        Vector2Int exitDir;
        for (int i = 0; i < path.Count; i++)
        {
            //Debug.Log("place track");
            Vector2Int cur = path[i];

            if (trainTrack.Contains(cur))
            {
                continue;
            }

            trainTrack.Add(cur);

            if (i > 0)
            {
                enterDir = path[i - 1] - cur;
            }
            else
            {
                enterDir = new Vector2Int(-1, 0);
            }

            if (i < path.Count - 1)
            {
                exitDir = path[i+1] - cur;
            }
            else
            {
                exitDir = new Vector2Int(1, 0);
            }

            if (exitDir.x == 0 && enterDir.x == 0)
            {
                grid[cur.x, cur.y].TrackTile = Instantiate(vTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;
            }
            else if (exitDir.y == 0 && enterDir.y == 0)
            {
                grid[cur.x, cur.y].TrackTile = Instantiate(hTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;
            }
            else if (enterDir + exitDir == new Vector2Int(1, -1))
            { 
                grid[cur.x, cur.y].TrackTile = Instantiate(tlTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;
            }
            else if (enterDir + exitDir == new Vector2Int(-1, -1))
            {
                grid[cur.x, cur.y].TrackTile = Instantiate(trTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;

            }
            else if (enterDir + exitDir == new Vector2Int(-1, 1))
            {
                grid[cur.x, cur.y].TrackTile = Instantiate(brTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;
            }
            else
            {
                grid[cur.x, cur.y].TrackTile = Instantiate(blTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].TrackTile.transform.parent = buildingParent;
            }
        }
    }


    public void AddStation(BuildingTemplateSO stationTemplate)
    {
        Vector2Int rand = trainTrack[UnityEngine.Random.Range(0, trainTrack.Count)];


        while (grid[rand.x, rand.y].GetTag() == null) 
        {
            rand = trainTrack[UnityEngine.Random.Range(0, trainTrack.Count)];
        }
        

        grid[rand.x, rand.y].BuiltObject = Instantiate(stationTemplate.mapPrefab, GridCoordToWorldPos(rand.x, rand.y), Quaternion.identity);
        grid[rand.x, rand.y].BuiltObject.GetComponent<StationTile>().station = stationPath.AddStation(stationTemplate);
    }

    void PlaceStartingTile()
    {
        Vector2Int coord = trainTrack[0];

        grid[coord.x, coord.y].loopStart = true;
        grid[coord.x, coord.y].BuiltObject = Instantiate(startTilePrefab, GridCoordToWorldPos(coord.x, coord.y), Quaternion.identity);

    }

    bool InBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

}

public enum TerrainType
{
    none = 0,
    grass = 1,
    mountain = 2,
    water = 3
}
