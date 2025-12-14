using UnityEngine;

public class SnappingPoint : MonoBehaviour
{ 
    DragManager dragManager;
    public float snapRange = 5;
    public GameObject occupiedGO;
    public string snapTag;
    public StationPath stationManager;



    public virtual void Awake()
    {
        dragManager = FindFirstObjectByType<DragManager>();
        dragManager.snappingpoints.Add(this);
        stationManager = FindFirstObjectByType<StationPath>();

        
    }


    // Update is called once per frame
    void Update()
    {
        

    }

    public virtual void NewStation()
    {

    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, snapRange);
    }

}
