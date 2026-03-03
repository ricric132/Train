using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StationChoice : MonoBehaviour
{
    public BuildingTemplateSO stationSO;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI shortDescriptionText;
    public StationPath stationPath;

    GameManager gameManager;
    StationSelection stationSelection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        stationSelection = gameManager.contractManager.stationSelection;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    public void Click()
    {
        stationSelection.SelectStation(stationSO);
    }

    public void UpdateText()
    {
        nameText.text = stationSO.name;
        shortDescriptionText.text = stationSO.description;
    }
}
