using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class Passenger : DragObj
{
    public GameObject infoPanelPrefab;
    public GameObject activeInfoPanel;
    public GameObject canvas;

    public PassengerInfo info;
    public PlayerManager playerManager;

    bool queueing;
    public Transform queueSpot;

    public Station station;

    public SpawnEffectPopup effectPopup;
    public Transform effectsCanvas;
    public GameObject destinationReachedIndicator;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;
    public TrainManager trainManager;
    public StationPath path;
    public PassengerGenerator passengerGenerator;
    public TriggerEffectHandler triggerEffectHandler;
    public bool stillActive = true;

    bool awakeRan = false;
    bool startRan = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Awake()
    {
        if (awakeRan)
        {
            return;
        }

        base.Awake();
        canOverlap = false;
        canvas = GameObject.Find("MainCanvas");

        effectPopup = GetComponent<SpawnEffectPopup>();
        awakeRan = true;
    }

    public override void Start()
    {
        if (startRan)
        {
            return;
        }

        base.Start();
        playerManager = gameManager.playerManager;
        trainManager = gameManager.trainManager;
        path = gameManager.stationPath;
        passengerGenerator = gameManager.passengerGenerator;
        triggerEffectHandler = gameManager.triggerEffectHandler;
        startRan = true;
    }

    public void ManualStart()
    {
        Awake();
        Start();
    }



    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (seat != null && !trainManager.stopped)
        {
            locked = true;
        }
        else
        {
            locked = false;
        }
        
        coinText.text = info.coins.ToString();
        timeText.text = info.stopsRemaining.ToString();

        /*
        if (activeInfoPanel != null)
        {
            activeInfoPanel.transform.position = Input.mousePosition;
        }
        */
    }

    public virtual void OnMouseEnter()
    {
        if (gameManager.mouseOverUI)
        {
            if (activeInfoPanel != null)
            {
                Destroy(activeInfoPanel);
                activeInfoPanel = null;
            }
            return;
        }
       
        if (activeInfoPanel != null)
        {
            Destroy(activeInfoPanel);
            activeInfoPanel = null;
        }

        activeInfoPanel = Instantiate(infoPanelPrefab, canvas.transform);
        activeInfoPanel.GetComponent<InfoPanel>().SetUp(info);
    }

    private void OnMouseExit()
    {
        if(activeInfoPanel != null)
        {
            Destroy(activeInfoPanel);
            activeInfoPanel = null;
        }
    }

    public void SetUp(PassengerInfo _info)
    {
        info = _info;
    }


    public virtual IEnumerator NextStation()
    {
        yield return StartCoroutine(NextStationAction());
        UpdateStationsRemaining(-1);
        yield return new WaitForSeconds(1f/gameManager.animationSimSpeed);

       
        
        //Debug.Log(info.GetFullName());
    }
    public virtual IEnumerator NextStationAction()
    {
        yield return null;
        //Debug.Log(info.GetFullName());
    }

    public override void OnSeated(Seat _seat)
    {
        base.OnSeated(_seat);

        if(seat == null)
        {
            seat = _seat;
            if(station != null)
            {
                station.RemoveFromQueue(this);
            }
            DoSeatedEffect(_seat);
        }
        else
        {
            seat = _seat;

        }
    }

    public virtual void DoSeatedEffect(Seat _seat)
    {
        StartCoroutine(triggerEffectHandler.TriggerOnBoard(this));
    }

    public void OnDepart()
    {
        path.GetCurStation().OnPassengerDepart(this);
        if (!ReachedStation())
        {
            playerManager.AddNegative(info.stopsRemaining);
            //DestinationReachedEffect();

        }
        else
        {
            playerManager.AddPositive(info.coins);
            DestinationReachedEffect();
        }

        Remove();
    }

    public void Remove()
    {
        passengerGenerator.UpdateSpeciesTableRemaining(info.species, 1);
        if (seat != null && seat.occupiedGO == gameObject)
        {
            seat.occupiedGO = null;
        }

        if (activeInfoPanel)
        {
            Destroy(activeInfoPanel);
        }
        Destroy(gameObject);
    }

    public virtual void DestinationReachedEffect()
    {

    }

    public bool ReachedStation()
    {
        return info.stopsRemaining == 0;
    }

    public override void HandleSnap(SnappingPoint snapPoint)
    {
        if (snapPoint.tag == "DepartPoint")
        {
            if (seat != null)
            {
                Debug.Log("departed");
                seat.occupiedGO = null;
                prevParentSnap.occupiedGO = null;
                transform.parent = snapPoint.transform;
                transform.localPosition = Vector3.zero;
                OnDepart();
            }
            else
            {
                ReturnToStart();
            }
        }
        else if (!canOverlap && snapPoint.occupiedGO != null && snapPoint.occupiedGO != gameObject)
        {
            ReturnToStart();
        }
        else if (seat != null)
        {
            ReturnToStart();
        }
        else
        {

            transform.parent = snapPoint.transform;
            transform.localPosition = Vector3.zero;
            if(prevParentSnap != null)
            {
                prevParentSnap.occupiedGO = null;
            }
            snapPoint.occupiedGO = gameObject;

            if (snapPoint.tag == "Seat")
            {
                OnSeated(snapPoint.gameObject.GetComponent<Seat>());
            }
            else
            {
                seat = null;
            }
        }
        
    }

    public bool UpdateStationsRemaining(int amount)
    {
        
        info.stopsRemaining += amount;
        info.stopsRemaining = Math.Max(0, info.stopsRemaining);
        effectPopup.SpawnPopup(effectsCanvas, amount, SpawnEffectPopup.popupType.time);

        if (ReachedStation())
        {
            destinationReachedIndicator.SetActive(true);
            return false;
        }
        return true;
    }

    public void UpdateCoins(int amount)
    {

        info.coins += amount;

        effectPopup.SpawnPopup(effectsCanvas, amount, SpawnEffectPopup.popupType.coin);
    }
}
