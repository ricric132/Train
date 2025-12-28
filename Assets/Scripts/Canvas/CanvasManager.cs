using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using static CameraManager;

public class CanvasManager : MonoBehaviour
{
    enum CanvasState{
        Default,
        NoHUD,
        Map
    }

    [SerializeField] TextMeshProUGUI negativeText;
    [SerializeField] TextMeshProUGUI positiveText;
    int storedPositive = 0;
    int storedQuota = 0;


    [SerializeField] List<GameObject> StationBarSlots;

    [SerializeField] List<GameObject> StationIconPrefabs;
    Dictionary<Station.Types, GameObject> stationTypetoIcon;

    List<GameObject> activeIcons = new List<GameObject>();

    [SerializeField] GameObject defaultCanvas;
    [SerializeField] GameObject shopCanvas;
    [SerializeField] GameObject contractCanvas;

    public GameObject effectsCanvas;

    [SerializeField] GameObject endOfDayPopup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateNegativeText(int num)
    {
        negativeText.text = num.ToString();
    }

    public void UpdatePositiveText(int num)
    {
        storedPositive = num;
        positiveText.text = storedPositive + "/" + storedQuota;
    }

    public void UpdateQuotaText(int num)
    {
        storedQuota = num;
        positiveText.text = storedPositive + "/" + storedQuota;
    }

    public void SetupStationsPreview(List<Station> stations, int currentStation)
    {
        for(int i = 0; i < activeIcons.Count; i++)
        {
            Destroy(activeIcons[i]);
        }

        activeIcons.Clear();

        for(int i = currentStation; (i < stations.Count && i < currentStation+5); i++)
        {
            GameObject icon = Instantiate(StationIconPrefabs[(int)stations[i].type]);
            icon.transform.parent = StationBarSlots[i - currentStation].transform;
            icon.transform.localPosition = Vector3.zero;
            activeIcons.Add(icon);
        }
    }

    public void SetCanvasMode(GameManager.GameState state)
    {
        if (state == GameManager.GameState.Default)
        {
            DisableCanvas();
            defaultCanvas.SetActive(true);
            return;
        }

        if (state == GameManager.GameState.PlayingAnimation)
        {
            DisableCanvas();
            defaultCanvas.SetActive(true);
            return;
        }

        if (state == GameManager.GameState.ShopMenu)
        {
            DisableCanvas();
            shopCanvas.SetActive(true);
            return;
        }

        if (state == GameManager.GameState.ContractMenu)
        {
            DisableCanvas();
            contractCanvas.SetActive(true);
            return;
        }
    }

    public void DisableCanvas()
    {
        //disables all ui
        defaultCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        contractCanvas.SetActive(false);
    }

    public void StationChoiceComplete()
    {
        SetCanvasMode(GameManager.GameState.Default);
    }

    public void OpenEndOfDayOverviewPopup()
    { 
        endOfDayPopup.SetActive(true);
    }

    public void SetupEndOfDayData(int quota, int coinAmt)
    {
        endOfDayPopup.GetComponent<EndOfDayPopup>().Setup(quota, coinAmt);

    }

    public void CloseEndOfDayOverviewPopup()
    {
        endOfDayPopup.SetActive(false);
    }
}
