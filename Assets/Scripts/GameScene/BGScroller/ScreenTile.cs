using UnityEngine;

public class ScreenTile : MonoBehaviour
{
    [SerializeField] Transform bg;
    [SerializeField] Transform mg;
    [SerializeField] Transform fg;

    [SerializeField] Sprite defaultTrack;

    public Station renderedStation;

    Vector2Int curCoord;

    public void RenderTile(Vector2Int coords)
    {
        if(coords == curCoord)
        {
            return;
        }

        //get fore ground and background later and render seperately on diff layers, for now just get cur tile
        MapTile curTile = GameManager.Instance.mapGrid.grid[coords.x, coords.y];


        Station curStation = curTile.GetStation();
        if(renderedStation && renderedStation != curStation)
        {
            renderedStation.gameObject.SetActive(false);
        }

        if (curStation)
        {
            curStation.transform.SetParent(transform);
            curStation.transform.localPosition = Vector3.zero;
            curStation.gameObject.SetActive(true);
            renderedStation = curStation;
        }
    }

}
