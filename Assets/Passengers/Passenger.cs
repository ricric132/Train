using System;
using System.Collections;
using TMPro;
using UnityEngine;


public class Passenger : DragObj
{
    public GameObject infoPanelPrefab;
    [HideInInspector] public GameObject activeInfoPanel;
    [HideInInspector] public GameObject canvas;

    [HideInInspector] public PassengerInfo info;
    [HideInInspector] public PlayerManager playerManager;

    bool queueing;
    [HideInInspector] public Transform queueSpot;

    [HideInInspector] public Station station;

    [HideInInspector] public SpawnEffectPopup effectPopup;
    public Transform effectsCanvas;
    public GameObject destinationReachedIndicator;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;
    [HideInInspector] public TrainManager trainManager;
    [HideInInspector] public StationPath path;
    [HideInInspector] public PassengerGenerator passengerGenerator;
    [HideInInspector] public TriggerEffectHandler triggerEffectHandler;
    [HideInInspector] public bool stillActive = true;

    bool awakeRan = false;
    bool startRan = false;

    public bool partOfPool = false;

    public PassengerAnimator passengerAnimator;

    

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
        passengerAnimator = GetComponent<PassengerAnimator>();


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
        
        if (timeText != null)
        {
            timeText.text = info.stopsRemaining.ToString();
        }

        if (coinText != null)
        {
            coinText.text = info.coins.ToString();
        }

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
        yield return new WaitForSeconds(0.5f / gameManager.animationSimSpeed);
        
        yield return StartCoroutine(NextStationAction());
        UpdateStationsRemaining(-1);
        yield return new WaitForSeconds(0.5f / gameManager.animationSimSpeed);

        //Debug.Log(info.GetFullName());
    }

    public virtual IEnumerator NextStationAction()
    {
        passengerAnimator.NewStationEffectAnim();
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

    public virtual void DoSeatedEffect(Seat _seat) // this should probably be a coroutine
    {
        passengerAnimator.OnboardAnim();
        StartCoroutine(triggerEffectHandler.TriggerOnBoard(this));
    }

    public virtual void OnDepart()
    {
        path.GetCurStation().OnPassengerDepart(this);
        StartCoroutine(triggerEffectHandler.TriggerOffBoard(this));
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
        if (partOfPool)
        {
            passengerGenerator.UpdateSpeciesTableRemaining(info.species, 1);
        }

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
                //Debug.Log("departed");

                //seat.occupiedGO = null;
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
            Seat snapPointSeat = null;
            snapPoint.gameObject.TryGetComponent<Seat>(out snapPointSeat);

            if (snapPointSeat != null) 
            {
                if (snapPointSeat.CheckActive())
                {
                    SnapTo(snapPoint);
                    OnSeated(snapPointSeat);
                }
                else
                {
                    ReturnToStart();
                }
            }
            else
            {
                SnapTo(snapPoint);
                seat = null;
            }
        }
    }

    public void SnapTo(SnappingPoint snapPoint)
    {
        transform.parent = snapPoint.transform;
        transform.localPosition = Vector3.zero;
        if (prevParentSnap != null)
        {
            prevParentSnap.occupiedGO = null;
        }
        snapPoint.occupiedGO = gameObject;
    }

    public virtual bool UpdateStationsRemaining(int amount)
    {
        
        info.stopsRemaining += amount;
        info.stopsRemaining = Math.Max(0, info.stopsRemaining);
        effectPopup.SpawnPopup(effectsCanvas, amount, SpawnEffectPopup.popupType.time);
        timeText.text = info.stopsRemaining.ToString();


        if (ReachedStation())
        {
            destinationReachedIndicator.SetActive(true);
            return false;
        }
        return true;
    }

    public virtual void UpdateCoins(int amount)
    {
        info.coins += amount;
        effectPopup.SpawnPopup(effectsCanvas, amount, SpawnEffectPopup.popupType.coin);
        coinText.text = info.coins.ToString();

    }

    public virtual void Warm()//should trigger the effects manager to gain extra effects
    {
        UpdateCoins(1);
    }

    public virtual void Chill()
    {
        UpdateCoins(3);
        UpdateStationsRemaining(1);
    }

}
