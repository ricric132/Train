using UnityEditor.SpeedTree.Importer;
using UnityEngine;
using UnityEngine.Splines.ExtrusionShapes;

public class SnappingPoint : MonoBehaviour
{
    public enum BoundShape{
        Circle,
        Box
    }

    DragManager dragManager;

    public BoundShape boundShape = BoundShape.Circle;
    public float snapRange = 5;
    public Vector2 boundingBox;

    public GameObject occupiedGO;
    public string snapTag;
    public StationPath stationManager;

    bool indicating;
    public GameObject availableIndicator;


    public virtual void Start()
    {
        dragManager = GameManager.Instance.dragManager;
        dragManager.snappingpoints.Add(this);
        stationManager = GameManager.Instance.stationPath;
    }


    // Update is called once per frame
    public virtual void Update()
    {
        if (availableIndicator)
        {
            availableIndicator.SetActive(indicating);
        }

    }

    public virtual void NewStation()
    {

    }
    public bool CheckSnap(Vector2 pos)
    {
        Vector2 posDiff = pos - ((Vector2)transform.position);
        if (boundShape == BoundShape.Circle)
        {
            if (posDiff.magnitude <= snapRange)
            {
                return true;
            }
        }

        if (boundShape == BoundShape.Box)
        {
            Vector2 adjustedPositionDiff = transform.InverseTransformPoint(pos);
            Debug.Log("adjusted " + adjustedPositionDiff);
            Debug.Log("base " + posDiff);
            if (Mathf.Abs(adjustedPositionDiff.x) <= boundingBox.x && Mathf.Abs(adjustedPositionDiff.y) <= boundingBox.y)
            {
                Debug.Log(posDiff);
                return true;
            }
        }

        return false;
    }

    public void ShowIndicator()
    {
        indicating = true;
    }

    public void StopIndicating()
    {
        indicating = false;
    }

    public virtual void OnDrawGizmos()
    {
        if (boundShape == BoundShape.Circle)
        {
            Gizmos.DrawWireSphere((Vector2)transform.position, snapRange);

        }

        if(boundShape == BoundShape.Box)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector2.zero, boundingBox*2);
        }
    }

}
