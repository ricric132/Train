using UnityEngine;

public class SnappingPoint : MonoBehaviour
{ 
    DragManager dragManager;
    public float snapRange = 5;
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
        Gizmos.DrawWireSphere(transform.position, snapRange);
    }

}
