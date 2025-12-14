using JetBrains.Annotations;
using System;
using System.Collections;
using System.Threading;
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

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI timeText;
    public TrainManager trainManager;
    public StationPath path;
    public PassengerGenerator passengerGenerator;
    public TriggerEffectHandler triggerEffectHandler;
    public bool stillActive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Awake()
    {
        base.Awake();
        canOverlap = false;
        canvas = GameObject.Find("MainCanvas");
        playerManager = FindFirstObjectByType<PlayerManager>();
        effectPopup = GetComponent<SpawnEffectPopup>();
        trainManager = FindFirstObjectByType<TrainManager>();
        path = FindFirstObjectByType<StationPath>();
        passengerGenerator = FindFirstObjectByType<PassengerGenerator>();
        triggerEffectHandler = FindFirstObjectByType<TriggerEffectHandler>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        coinText.text = info.coins.ToString();
        timeText.text = info.stopsRemaining.ToString();

        if (activeInfoPanel != null)
        {
            activeInfoPanel.transform.position = Input.mousePosition;
        }
    }

    public virtual void OnMouseEnter()
    {
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
        yield return new WaitForSeconds(1f);

       
        
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
        StartCoroutine(triggerEffectHandler.TriggerOnBoard());
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
        if(seat.occupiedGO == gameObject)
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

    bool ReachedStation()
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
                transform.parent = snapPoint.transform;
                transform.localPosition = Vector3.zero;
                OnDepart();
            }
            else
            {
                transform.position = startPos;
                transform.parent = startParent;
            }
        }
        else if (!canOverlap && snapPoint.occupiedGO != null && snapPoint.occupiedGO != gameObject)
        {
            transform.position = startPos;
            transform.parent = startParent;
        }
        else if (seat != null)
        {
            transform.position = startPos;
            transform.parent = startParent;
        }
        else
        {
            transform.parent = snapPoint.transform;
            transform.localPosition = Vector3.zero;
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
            OnDepart();
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
