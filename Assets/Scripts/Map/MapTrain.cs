using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MapTrain : MonoBehaviour
{
    MapGrid map;
    int curTile;
    int nextTile = 1;
    float speed = 3;
    float moveProgress = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        map = FindFirstObjectByType<MapGrid>();
        curTile = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void Move()
    {
        Vector2 curPos = map.GridCoordToWorldPos(CurCoords().x, CurCoords().y, centred:true);
        Vector2 nextPos = map.GridCoordToWorldPos(NextCoords().x, NextCoords().y, centred:true);
        moveProgress += Time.deltaTime * speed;
        transform.position = Vector2.Lerp(curPos, nextPos, Mathf.Clamp(moveProgress, 0, 1));
        if(moveProgress >= 1)
        {
            curTile = nextTile;
            nextTile = (nextTile + 1) % map.trainTrack.Count;
            moveProgress = 0;
        }
    }

    public Station CheckStation()
    {
        return map.grid[CurCoords().x, CurCoords().y].GetStation();
    }

    public bool CheckLooped()
    {
        return map.grid[CurCoords().x, CurCoords().y].loopStart;
    }

    Vector2Int CurCoords()
    {
        return map.trainTrack[curTile];
    }
    Vector2Int NextCoords()
    {
        return map.trainTrack[nextTile];
    }

}
