using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Default,
        PlayingAnimation,
        EscapeMenu,
        ShopMenu
    }

    [SerializeField] CanvasManager canvasManager;
    [SerializeField] TrainManager trainManager;
    [SerializeField] StationPath stationPath;

    [SerializeField] StationSelection stationSelection;
    [SerializeField] PlayerManager playerManager;

    [SerializeField] ShopManager shopManager;

    int quotaAmount;
    int startingQuota = 5;

    InputAction map;

    GameState prevState;
    GameState state = GameState.Default;

    [SerializeField] List<BuildingTemplateSO> stationTemplateSOs = new List<BuildingTemplateSO>();

    [SerializeField] List<StationChoice> stationChoices = new List<StationChoice>();

    [SerializeField] TextMeshProUGUI clockText;
    public ClockTime dayClock;
    public float clockTimer = 0.0f;
    [SerializeField] float clockSpeed = 10;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasManager = FindFirstObjectByType<CanvasManager>();
        trainManager = FindFirstObjectByType<TrainManager>();
        playerManager = FindFirstObjectByType<PlayerManager>();
        shopManager = FindFirstObjectByType<ShopManager>();


        dayClock = new ClockTime(6*60, 24*60);
        state = GameState.Default;
        prevState = GameState.Default;

        map = InputSystem.actions.FindAction("Map");
        map.Enable();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        UpdateClock();
    }

    void HandleInputs()
    {
        if (map.WasPressedThisFrame())
        {
            ToggleShop();
        }
    }

    void UpdateClock()
    {
        if (trainManager.stopped)
        {
            return;
        }

        clockTimer += Time.deltaTime * clockSpeed;
        clockText.text = dayClock.GetString(clockTimer);

        if (dayClock.CheckDayEnd(clockTimer))
        {
            Debug.Log("day end");
            StartCoroutine(EndDay());
        }
    }

    void ToggleShop()
    {
        if(state != GameState.ShopMenu)
        {
            prevState = state;
            state = GameState.ShopMenu;
        }
        else
        {
            state = prevState;
        }

        canvasManager.SetCanvasMode(state);
    }

    void GenerateStationChoices()
    {
        int numChoices = 3;

        List<BuildingTemplateSO> options = stationTemplateSOs;


        
        for(int i = 0; i < stationChoices.Count; i++)
        {
            List<KeyValuePair<BuildingTemplateSO, float>> oddsTable = CreateOddsTable(options);
            float rng = Random.value;

            foreach(var item in oddsTable)
            {
                if(item.Value >= rng)  //fix this 
                {
                    stationChoices[i].stationSO = item.Key;
                    options.Remove(item.Key);
                    break;
                }
            }
        }
    }

    List<KeyValuePair<BuildingTemplateSO, float>> CreateOddsTable(List<BuildingTemplateSO> options)
    {
        List<KeyValuePair<BuildingTemplateSO, float>> oddsTable = new List<KeyValuePair<BuildingTemplateSO, float>>();

        float runningTotal = 0;
        for(int i = 0; i < options.Count; i++)
        {
            runningTotal += options[i].frequency; 
            oddsTable.Add(new KeyValuePair<BuildingTemplateSO, float>(options[i], runningTotal));
        }

        for(int i = 0; i < oddsTable.Count; i++)
        {
            oddsTable[i] = new KeyValuePair<BuildingTemplateSO, float>(oddsTable[i].Key, oddsTable[i].Value/runningTotal);
        }

        return oddsTable;
    }

    public void NextStation()
    {
        //StartCoroutine(GoToNextStation());
        if (trainManager.stopped)
        {
            StartCoroutine(LeaveStation());
        }
    }

    public IEnumerator GoToNextStation()
    {
        prevState = state;
        state = GameState.PlayingAnimation;


        yield return StartCoroutine(trainManager.SimulateLeaveStation());

        stationPath.RemovePrevStation();
        bool nextStationFound = stationPath.NextStation();
        if (nextStationFound)
        {
            stationPath.GenerateStation();
        }
        else
        {
            stationPath.GenerateUpgradeStation();
        }

        yield return StartCoroutine(trainManager.SimulateEnterStation());

        if (!nextStationFound)
        {
            LoopComplete();
        }
        else
        {
            Station curStation  = stationPath.GetCurStation();
            yield return StartCoroutine(curStation.StationEnter());
            yield return StartCoroutine(trainManager.SimulatePassengerEffects(curStation));
        }

        prevState = state;
        state = GameState.Default;
    }

    public IEnumerator EnterStation(Station station)
    {
        prevState = state;
        state = GameState.PlayingAnimation;

        stationPath.GenerateStation(station);

        yield return StartCoroutine(trainManager.SimulateEnterStation());

        Station curStation = station;
        yield return StartCoroutine(curStation.StationEnter());
        yield return StartCoroutine(trainManager.SimulatePassengerEffects(curStation));

        prevState = state;
        state = GameState.Default;
    }

    public IEnumerator EndDay()
    {

        prevState = state;
        state = GameState.PlayingAnimation;

        trainManager.stopped = true;
        stationPath.GenerateUpgradeStation();
        yield return StartCoroutine(trainManager.SimulateEnterStation());

        prevState = state;
        state = GameState.Default;

        LoopComplete();

    }

    public void LoopComplete()
    {
        stationSelection.RandomizeStationSelect();
        GenerateStationChoices();

        ToggleShop(); // change later
        shopManager.SetUp();
        QuotaIncreaseFunction();

        /*
        if (QuotaReached())
        {

        }
        else
        {

        }
        */
    }

    public IEnumerator StartNextDay()
    {
        ToggleShop();
        clockTimer = 0.0f;
        yield return LeaveStation();
        trainManager.stopped = false;

    }

    public IEnumerator LeaveStation()
    {
        prevState = state;
        state = GameState.PlayingAnimation;

        yield return StartCoroutine(trainManager.SimulateLeaveStation());
        stationPath.RemovePrevStation();

        prevState = state;
        state = GameState.Default;
    }



    void QuotaIncreaseFunction()
    {
        if(quotaAmount == 0)
        {
            quotaAmount = startingQuota;
        }
        else
        {
            quotaAmount += (int)Mathf.Log(quotaAmount);
        }
        canvasManager.UpdateQuotaText(quotaAmount);
    }

    bool QuotaReached()
    {
        return quotaAmount < playerManager.GetPositive();
    }
}
