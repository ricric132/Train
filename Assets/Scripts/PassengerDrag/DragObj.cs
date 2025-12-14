using System.Xml.Serialization;
using UnityEngine;

public class DragObj : MonoBehaviour
{
    public bool canOverlap = false;

    public DragManager dragManager;
    public bool isDragging = false;
    public Vector3 dragOffset;
    public Seat seat;

    public Vector3 startPos;
    public Transform startParent;

    public bool locked;

    public virtual void Awake()
    {
       dragManager = FindFirstObjectByType<DragManager>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (isDragging)
        {
            dragManager.selectedDragObject = gameObject;
            transform.position = dragManager.getMousePos() + dragOffset;
        }
    }

    private void OnMouseDown()
    {
        if (locked)
        {
            return;
        }

        startPos = transform.position;
        startParent = transform.parent;

        dragOffset = transform.position - dragManager.getMousePos(); 
        isDragging = true;
        SnappingPoint parentSnap = null;
        //seat = null;

        if(transform.parent != null && transform.parent.TryGetComponent<SnappingPoint>(out parentSnap))
        {
            parentSnap.occupiedGO = null;
        }
        transform.parent = null;
    }

    private void OnMouseUp()
    {
        if (!isDragging)
        {
            return;
        }

        isDragging = false;
        SnappingPoint snapPoint = dragManager.checkSnap();

        if(snapPoint == null)
        {
            ReturnToStart();
        }
        else
        {
            HandleSnap(snapPoint);
        }
    }

    void ReturnToStart()
    {
        transform.position = startPos;
        transform.parent = startParent;
    }

    public virtual void HandleSnap(SnappingPoint snapPoint)
    {
        if (!canOverlap && snapPoint.occupiedGO != null && snapPoint.occupiedGO != gameObject)
        {
            transform.position = startPos;
            transform.parent = startParent;
        }
        else
        {
            transform.parent = snapPoint.transform;
            transform.localPosition = Vector3.zero;
            snapPoint.occupiedGO = gameObject;
        }
    }

    public virtual void OnSeated(Seat  _seat)
    {
        //locked = true;
    }




}
