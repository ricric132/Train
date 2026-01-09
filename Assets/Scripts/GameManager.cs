using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Default,
        PlayingAnimation,
        EscapeMenu,
        ShopMenu,
        ContractMenu
    }

    public CameraManager cameraManager;
    public CanvasManager canvasManager;
    public TrainManager trainManager;
    public StationPath stationPath;

    public StationSelection stationSelection;
    public PlayerManager playerManager;

    public ShopManager shopManager;
    public ContractManager contractManager;
    public PassengerGenerator passengerGenerator;

    public TriggerEffectHandler triggerEffectHandler;

    public DragManager dragManager;

    int upgradeCurrencyGain;
    int baseUpgradeCurrencyGain = 10;
    public int GetBaseCurrencyGain()
    {
        return baseUpgradeCurrencyGain;
    }
    public float currencyGainMult = 1;

    int quotaAmount;
    int baseQuota;
    public int GetBaseQuota()
    {
        return baseQuota;
    }
    public float quotaMult = 1;
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

    public bool mouseOverUI;

    public float trainMoveSimSpeed = 1;
    public float animationSimSpeed = 1;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        cameraManager = FindFirstObjectByType<CameraManager>();
        canvasManager = FindFirstObjectByType<CanvasManager>();
        trainManager = FindFirstObjectByType<TrainManager>();
        playerManager = FindFirstObjectByType<PlayerManager>();
        shopManager = FindFirstObjectByType<ShopManager>();
        contractManager = FindFirstObjectByType<ContractManager>();
        passengerGenerator = FindFirstObjectByType<PassengerGenerator>();
        triggerEffectHandler = FindFirstObjectByType<TriggerEffectHandler>();
        dragManager = FindFirstObjectByType<DragManager>();


        dayClock = new ClockTime(6 * 60, 24 * 60);
        state = GameState.Default;
        prevState = GameState.Default;
        upgradeCurrencyGain = baseUpgradeCurrencyGain;

        map = InputSystem.actions.FindAction("Map");
        map.Enable();

        baseQuota = startingQuota;

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvasManager.UpdateQuotaText(baseQuota);

    }

    // Update is called once per frame
    void Update()
    {
        HandleInputs();

        UpdateClock();
    }

    void HandleInputs()
    {
        mouseOverUI = EventSystem.current.IsPointerOverGameObject();

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

        clockTimer += Time.deltaTime * clockSpeed * trainMoveSimSpeed;
        clockText.text = dayClock.GetString(clockTimer);

        if (dayClock.CheckDayEnd(clockTimer))
        {
            Debug.Log("day end");
            StartCoroutine(EndDay());
        }
    }

    public void ToggleShop()
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
    public void ToggleContractMenu()
    {
        if (state != GameState.ContractMenu)
        {
            prevState = state;
            state = GameState.ContractMenu;
        }
        else
        {
            state = prevState;
        }

        canvasManager.SetCanvasMode(state);
    }

    public void SetupContractMenu()
    {
        contractManager.Setup();
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
        //check for no passengers ready to disembark 
        //StartCoroutine(GoToNextStation());
        if (trainManager.stopped)
        {
            StartCoroutine(LeaveStation());
        }
    }

    public IEnumerator EnterStation(Station station)
    {
        prevState = state;
        state = GameState.PlayingAnimation;

        stationPath.GenerateStation(station);

        yield return StartCoroutine(trainManager.SimulateEnterStation());

        Station curStation = station;

        yield return StartCoroutine(trainManager.SimulatePassengerEffects(curStation));
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(curStation.StationEnter());
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(triggerEffectHandler.TriggerOnEnterStation(curStation));

        prevState = state;
        state = GameState.Default;
    }

    public IEnumerator EndDay()
    {

        prevState = state;
        state = GameState.PlayingAnimation;

        trainManager.stopped = true;
        LoopComplete();

        stationPath.GenerateUpgradeStation();
        yield return StartCoroutine(trainManager.SimulateEnterStation());

        trainManager.DecrimentContractDays();
        canvasManager.OpenEndOfDayOverviewPopup();
        canvasManager.SetupEndOfDayData(quotaAmount, playerManager.GetPositive());
        playerManager.UpdateMoney(upgradeCurrencyGain);


        prevState = state;
        state = GameState.Default;

    }

    public IEnumerator Sacrifice()
    {

        yield return null;
    }


    public void LoopComplete()
    {
        //stationSelection.RandomizeStationSelect();
        //GenerateStationChoices();
        Debug.Log("complete");
        shopManager.SetUp();

        //ToggleShop(); // change later
    }

    public void NextDay()
    {
        StartCoroutine(StartNextDay());
    }

    public IEnumerator StartNextDay()
    {
        ToggleContractMenu();
        QuotaIncreaseFunction();

        clockTimer = 0.0f;
        yield return StartCoroutine(LeaveStation());
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
        if(baseQuota == 0)
        {
            baseQuota = startingQuota;
        }
        else
        {
            baseQuota = GetNextBaseQuota();
        }

        quotaAmount = (int)(baseQuota * quotaMult);
        canvasManager.UpdateQuotaText(quotaAmount);
    }

    public int GetNextBaseQuota()
    {
        return baseQuota + (int)Mathf.Log(baseQuota);
    }

    public void UpdateQuotaMult(float amount)
    {
        quotaMult += amount;
        quotaAmount = (int)(baseQuota * quotaMult); 
        canvasManager.UpdateQuotaText(quotaAmount);
    }

    public void UpdateCurrencyMult(float amount)
    {
        currencyGainMult += amount;
        upgradeCurrencyGain = (int)(baseUpgradeCurrencyGain * currencyGainMult);
    }

    bool QuotaReached()
    {
        return quotaAmount < playerManager.GetPositive();
    }

    public bool RecieveInput()
    {
        return state != GameState.PlayingAnimation;
    }

}
