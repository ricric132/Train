using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StationChoice : MonoBehaviour
{
    public BuildingTemplateSO stationSO;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI shortDescriptionText;
    public StationPath stationPath;

    CanvasManager canvasManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stationPath = FindFirstObjectByType<StationPath>();
        canvasManager = FindFirstObjectByType<CanvasManager>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void Click()
    {
        stationPath.AddStation(stationSO);
        canvasManager.StationChoiceComplete();
    }

    public void UpdateText()
    {
        nameText.text = stationSO.name;
        shortDescriptionText.text = stationSO.description;
    }
}
