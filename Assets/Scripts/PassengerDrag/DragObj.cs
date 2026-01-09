using System.Xml.Serialization;
using UnityEngine;

public class DragObj : MonoBehaviour
{
    public bool canOverlap = false;

    [HideInInspector] public DragManager dragManager;
    [HideInInspector] public bool isDragging = false;
    [HideInInspector] public Vector3 dragOffset;
    [HideInInspector] public Seat seat;

    [HideInInspector] public Vector3 startPos;
    [HideInInspector] public Transform startParent;
    [HideInInspector] public SnappingPoint prevParentSnap;

    [HideInInspector] public bool locked;

    public GameObject visual;
    public Vector3 defaultScale;
    public Vector3 pickupScale;

    [HideInInspector] public GameManager gameManager;

    public virtual void Awake()
    {
       defaultScale = visual.transform.localScale;

       pickupScale = defaultScale * 1.2f;
       
    }

    public virtual void Start()
    {
        gameManager = GameManager.Instance;
        dragManager = FindFirstObjectByType<DragManager>();
    }


    // Update is called once per frame
    public virtual void Update()
    {
        if (isDragging)
        {
            dragManager.selectedDragObject = gameObject;
            transform.position = dragManager.GetMousePos() + dragOffset;
        }
    }

    private void OnMouseDown()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        if (locked)
        {
            return;
        }


        startPos = transform.localPosition;
        startParent = transform.parent;

        visual.transform.localScale = pickupScale;

        dragOffset = transform.position - dragManager.GetMousePos(); 
        isDragging = true;

        transform.parent.TryGetComponent<SnappingPoint>(out prevParentSnap);
        dragManager.HighlightPlacableSpots(prevParentSnap);
        //seat = null;
        /*
        if(transform.parent != null && transform.parent.TryGetComponent<SnappingPoint>(out parentSnap))
        {
            parentSnap.occupiedGO = null;
        }
        */
        transform.parent = null;
    }

    private void OnMouseUp()
    {
        if (gameManager.mouseOverUI)
        {
            return;
        }
        if (!isDragging)
        {
            return;
        }

        visual.transform.localScale = defaultScale;
        isDragging = false;
        SnappingPoint snapPoint = dragManager.CheckSnap();

        dragManager.UnhighlightAll();

        if(snapPoint == null)
        {
            ReturnToStart();
        }
        else
        {
            HandleSnap(snapPoint);
        }
    }

    public void ReturnToStart()
    {
        transform.parent = startParent;
        transform.localPosition = startPos;
    }

    public virtual void HandleSnap(SnappingPoint snapPoint)
    {
        if (!canOverlap && snapPoint.occupiedGO != null && snapPoint.occupiedGO != gameObject)
        {
            ReturnToStart();
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
