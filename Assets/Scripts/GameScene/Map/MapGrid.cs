using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
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
        
        foreach(Vector2Int track in trainTrack)
        {
            Debug.Log(grid[track.x, track.y].GetStation());
        }

    }

    public void Build(Vector2Int coords, Rotation rotation, BuildingTemplateSO template)
    {
        GameObject build = Instantiate(template.mapPrefab, GridCoordToWorldPos(coords.x, coords.y), Quaternion.identity);
        build.transform.parent = buildingParent;
        

        for(int i = 0; i < template.occupiedSpaces.Count; i++)
        {
            Vector2Int offset = template.occupiedSpaces[i];
            grid[coords.x + offset.x, coords.y + offset.y].builtObject = build;
            grid[coords.x + offset.x, coords.y + offset.y].empty = false;
            Debug.Log("Built " + grid[coords.x + offset.x, coords.y + offset.y].builtObject);
        }

        if(template.GetTag() == "Station")
        {
            build.GetComponent<StationTile>().station = stationPath.AddStation(template);
            build.GetComponent<StationTile>().coords = coords;
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
        float stepSize = 0.01f;
        for(int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                float value = Mathf.PerlinNoise(i * stepSize + origin.x, j * stepSize + origin.y);

                if(value < 0.3)
                {
                    //water
                }
                else if(value < 0.8)
                {
                    //grass
                }
                else
                {
                    //mountain
                }
            }
        }
    }

    void SetTerrain(Vector2Int coords, TerrainType terrainType)
    {

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
            Debug.Log("place track");
            Vector2Int cur = path[i];
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
                grid[cur.x, cur.y].builtObject = Instantiate(vTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;
            }
            else if (exitDir.y == 0 && enterDir.y == 0)
            {
                grid[cur.x, cur.y].builtObject = Instantiate(hTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;
            }
            else if (enterDir + exitDir == new Vector2Int(1, -1))
            { 
                grid[cur.x, cur.y].builtObject = Instantiate(tlTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;
            }
            else if (enterDir + exitDir == new Vector2Int(-1, -1))
            {
                grid[cur.x, cur.y].builtObject = Instantiate(trTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;

            }
            else if (enterDir + exitDir == new Vector2Int(-1, 1))
            {
                grid[cur.x, cur.y].builtObject = Instantiate(brTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;
            }
            else
            {
                grid[cur.x, cur.y].builtObject = Instantiate(blTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                grid[cur.x, cur.y].builtObject.transform.parent = buildingParent;
            }
        }
    }


    void GenTracksOnTile(GenGridTile[,] gen, int x, int y)
    {
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if(i == 0 && j == 0)
                {
                    continue;
                }
                if (x+i < 0 || x+i >= gen.GetLength(0) || y + j < 0 || y + j >= gen.GetLength(1) || !gen[x + i, y + j].inside || !gen[x + i, y].inside || !gen[x, y + j].inside)
                {
                    grid[x * 3 + 1 + i + borderSize, y * 3 + 1 + j + borderSize].track = true;
                    trainTrack.Add(new Vector2Int(x * 3 + 1 + i + borderSize, y * 3 + 1 + j + borderSize));
                }
            }
        }
        //grid[x * 3, y * 3].track = true;
    }

    void OrientateTracks()
    {
        Vector2Int enterDir = Vector2Int.zero;
        Vector2Int exitDir = Vector2Int.zero;
        Vector2Int startingPoint = trainTrack[UnityEngine.Random.Range(0, trainTrack.Count)];

        trainTrack.Clear(); 

        Vector2Int cur = startingPoint;

        bool looped = false;

        while (looped == false)
        {
            trainTrack.Add(cur);
            for(int i = 0; i < directions.Count; i++)
            {
                Vector2Int checkCoord = cur + directions[i];
                if (InBounds(checkCoord.x, checkCoord.y) && grid[checkCoord.x, checkCoord.y].track == true)
                {
                    if (enterDir == Vector2Int.zero)
                    {
                        enterDir = directions[i];
                    }
                    else if (directions[i] != enterDir)
                    {
                        exitDir = directions[i];

                        if (exitDir.x == 0 && enterDir.x == 0)
                        {
                            grid[cur.x, cur.y].builtObject = Instantiate(vTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                        }
                        else if (exitDir.y == 0 && enterDir.y == 0)
                        {
                            grid[cur.x, cur.y].builtObject = Instantiate(hTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                        }
                        else if (enterDir + exitDir == new Vector2Int(1, -1))
                        {

                            grid[cur.x, cur.y].builtObject = Instantiate(tlTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);
                            
                        }
                        else if (enterDir + exitDir == new Vector2Int(-1, -1))
                        {
                            grid[cur.x, cur.y].builtObject = Instantiate(trTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);

                        }
                        else if (enterDir + exitDir == new Vector2Int(-1, 1))
                        {
                            grid[cur.x, cur.y].builtObject = Instantiate(brTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);

                        }
                        else
                        {
                            grid[cur.x, cur.y].builtObject = Instantiate(blTrackTilePrefab, GridCoordToWorldPos(cur.x, cur.y), Quaternion.identity);  
                        }
                    }
                }
            }

            cur = cur + exitDir;
            enterDir = -exitDir;

            if(cur == startingPoint)
            {
                looped = true;
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
        

        grid[rand.x, rand.y].builtObject = Instantiate(stationTemplate.mapPrefab, GridCoordToWorldPos(rand.x, rand.y), Quaternion.identity);
        grid[rand.x, rand.y].builtObject.GetComponent<StationTile>().station = stationPath.AddStation(stationTemplate);
    }

    void PlaceStartingTile()
    {
        Vector2Int coord = trainTrack[0];

        grid[coord.x, coord.y].loopStart = true;
        grid[coord.x, coord.y].builtObject = Instantiate(startTilePrefab, GridCoordToWorldPos(coord.x, coord.y), Quaternion.identity);

    }

    bool InBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
    }

    //ARCHIVED STUFF//////////////////////////////////////////////////////////////////////////////////////
    /*
    void PlaceStartingStations()
    {
        if (RunBuffData.selectedConductor == null)
        {
            for (int i = 0; i < 3; i++)
            {
                Vector2Int rand = trainTrack[(i + 1) * (trainTrack.Count / startingStations - 2)];


                BuildingTemplateSO randStation = GameManager.Instance.stationPath.GetRandomStation();

                grid[rand.x, rand.y].go = Instantiate(randStation.mapPrefab, GridCoordToWorldPos(rand.x, rand.y), Quaternion.identity);
                grid[rand.x, rand.y].go.GetComponent<StationTile>().station = stationPath.AddStation(randStation);
                grid[rand.x, rand.y].go.transform.parent = transform;
            }
        }
        else
        {
            for (int i = 0; i < RunBuffData.selectedConductor.startingBuildings.Count; i++)
            {
                Vector2Int rand = trainTrack[(i + 1) * (trainTrack.Count / startingStations - 2)];

                
                while (grid[rand.x, rand.y].hasStation)
                {
                    rand = trainTrack[UnityEngine.Random.Range(0, trainTrack.Count)];
                }
                

                BuildingTemplateSO randStation = RunBuffData.selectedConductor.startingBuildings[i];

                grid[rand.x, rand.y].go = Instantiate(randStation.mapPrefab, GridCoordToWorldPos(rand.x, rand.y), Quaternion.identity);
                grid[rand.x, rand.y].go.GetComponent<StationTile>().station = stationPath.AddStation(randStation);
                grid[rand.x, rand.y].go.transform.parent = transform;
            }
        }
    }
    void SectionedGeneration()
    {
        GenGridTile[,] gen = mapGen.GenTrackErosionMap((width - borderSize * 2) / 3, (height - borderSize * 2) / 3, generationSteps); //generates a grid to be upscaled and offset
        for (int i = 0; i < gen.GetLength(0); i++)
        {
            for (int j = 0; j < gen.GetLength(1); j++)
            {
                if (gen[i, j].inside)
                {
                    GenTracksOnTile(gen, i, j);
                }
            }
        }
    }
    void RandomWalkGeneration()
    {
        //Vector2Int missingCorner = new Vector2Int(UnityEngine.Random.Range(0, 2), UnityEngine.Random.Range(0, 2));

        List<Vector2Int> corners = new List<Vector2Int>() { new Vector2Int(0, 0), new Vector2Int(1, 0) , new Vector2Int(1, 1) , new Vector2Int(0, 1) };
        List<Vector2Int> randomSpots = new List<Vector2Int>();

        for (int i = 0; i < corners.Count; i++)
        {
            Vector2Int rand = corners[i] * width;
            if (corners[i].x == 0){
                rand.x += UnityEngine.Random.Range(cornerZoneMin, cornerZoneMax);
            }
            else
            {
                rand.x -= UnityEngine.Random.Range(cornerZoneMin, cornerZoneMax);
            }

            if (corners[i].y == 0)
            {
                rand.y += UnityEngine.Random.Range(cornerZoneMin, cornerZoneMax);
            }
            else
            {
                rand.y -= UnityEngine.Random.Range(cornerZoneMin, cornerZoneMax);
            }

            randomSpots.Add(corners[i] + rand);
            
        }

        for(int i = 0; i < randomSpots.Count; i++)
        {
            Debug.Log(randomSpots[i] + " to " + randomSpots[(i + 1) % randomSpots.Count]);
            //DirectConnect(randomSpots[i], randomSpots[(i + 1) % randomSpots.Count]);
            CreateRandomWalk(randomSpots[i], randomSpots[(i + 1) % randomSpots.Count]);
        }
    }

    void CreateRandomWalk(Vector2Int start, Vector2Int end)
    {
        
        Vector2Int primaryDir;
        Vector2Int dir = end - start;
        if(Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            if(dir.x < 0)
            {
                primaryDir = new Vector2Int(-1, 0);
            }
            else
            {
                primaryDir = new Vector2Int(1, 0);
            }
        }
        else
        {
            if (dir.y < 0)
            {
                primaryDir = new Vector2Int(0, -1);
            }
            else
            {
                primaryDir = new Vector2Int(0, 1);
            }
        }



        Vector2Int cur = start;
        HashSet<Vector2Int> curPath;

        
        while (cur != end)
        {
            if(cur.x == end.x || cur.y == end.y)
            {
                DirectConnect(cur, end);
            }
            //Debug.Log(cur);
            grid[cur.x, cur.y].empty = false;
            List<Vector2Int> validDirs = new List<Vector2Int>();
            for (int i = 0; i < directions.Count; i++)
            {
                Vector2Int checkDir = directions[i];
                Vector2Int checkNext = cur + checkDir;
                bool valid = true;

                if (checkDir != -primaryDir && checkNext.x >= 2 && checkNext.x < width && checkNext.y >= 2 && checkNext.y < height)
                {
                    Vector2Int perp = new Vector2Int(-checkDir.y, -checkDir.x);

                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = 0; y < 2; y++)
                        {
                            Vector2Int temp = checkNext + checkDir * y + perp * x;
                            //Debug.Log("temp = "+temp);

                            if (temp.x < width && temp.x >= 0 && temp.y >= 0 && temp.y < height && !grid[temp.x, temp.y].empty)
                            {
                                valid = false;
                            }
                        }
                    }
                }
                else
                {
                    valid = false;
                }

                if (valid)
                {
                    validDirs.Add(checkDir);
                }

            }

            if(validDirs.Count > 0)
            {
                dir = validDirs[UnityEngine.Random.Range(0, validDirs.Count)];
                cur += dir;
            }
            else
            {
                Debug.Log("no space");
                return;
            }

            
        }
        
    }

    void DirectConnect(Vector2Int start, Vector2Int end)
    {
        while(start.x != end.x)
        {
            grid[start.x, start.y].empty = false;
            if(start.x < end.x)
            {
                start.x++;
            }
            else
            {
                start.x--;
            }
        }

        while (start.y != end.y)
        {
            grid[start.x, start.y].empty = false;
            if (start.y < end.y)
            {
                start.y++;
            }
            else
            {
                start.y--;
            }
        }
    }

    
    //List<List<Vector2Int>> GetAllPaths(Vector2Int start, Vector2Int end, int maxSteps)
    //{
        //List<List<Vector2Int>> AllPaths = new List<List<Vector2Int>>();


    //}
    



    void GenerateTrainTrack()
    {
        
    }
    
    void CellularAutomataGenerate(MapTile[,] grid, int steps)
    {
        
        grid[grid.GetLength(0) / 2, grid.GetLength(1) / 2].inside = true;

        for(int i = 0; i < steps; i++)
        {
            MapTile[,] newGrid = CellularAutomataStep(grid);
            grid = newGrid;
        }
    }

    MapTile[,] CellularAutomataStep(MapTile[,] grid)
    {
        MapTile[,] changeGrid = grid.Clone() as MapTile[,];
        
        for (int x = 0; x < grid.GetLength(1); x++)
        {
            for(int y = 0;  y < grid.GetLength(0); y++)
            {
                MapTile newTile = CellularAutomataRule(grid, x, y);
                changeGrid[y, x] = newTile;
            }
        }

        return changeGrid;
    }

    MapTile CellularAutomataRule(MapTile[,] grid, int x, int y)
    {

        int adjInside = 0;
        MapTile current = grid[y, x];

        if (current.inside)
        {
            return current;
        }

        for(int i = -1; i <= 1; i++)
        {
            for(int j = -1; j <= 1; j++)
            {
                if(i == 0 && j == 0) // gets self
                {
                    continue;
                }
                if(x+j >= grid.GetLength(1) || x+j < 0 || y+i >= grid.GetLength(0) || y+i < 0) //out of bounds
                {
                    continue;
                }
                else if (i==0 || j==0) //adjacent
                {
                    if(grid[y + i, x + j].inside)
                    {
                        adjInside++;
                    }
                }
            }
        }

        float chance = adjInside * 0.25f;

        if(UnityEngine.Random.value < chance)
        {
            current.inside = true;
        }

        return current;
        
    }

    private GameObject MakeLine(Vector3[] vertices)
    {
        GameObject o = new GameObject();
        LineRenderer line = o.AddComponent<LineRenderer>().GetComponent<LineRenderer>();
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        Material mat = new Material(shader) { color = new Color(0, 0, 0, 0.5f) };
        line.material = mat;
        line.startWidth = 0.5f;
        line.endWidth = 0.5f;
        line.positionCount = vertices.Length;
        line.SetPositions(vertices);
        return o;
    }
    */

}