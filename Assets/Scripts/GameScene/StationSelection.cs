using System.Collections.Generic;
using UnityEngine;


public class StationSelection : MonoBehaviour
{ 
    [SerializeField] List<StationChoice> stationChoiceSlots;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OfferSelection()
    {
        gameObject.SetActive(true);
    }

    public void RandomizeStationChoices()
    {
        List<BuildingTemplateSO> generatedStations = gameManager.stationGenerator.GenerateStationChoices(stationChoiceSlots.Count);
        //Debug.Log("station choice count: " + stationChoiceSlots.Count);
        //Debug.Log("count: " + generatedStations.Count);
        for (int i = 0; i < stationChoiceSlots.Count; i++)
        {
            //Debug.Log("i = " + i);

            stationChoiceSlots[i].stationSO = generatedStations[i];
        }

    }

    public void SelectStation(BuildingTemplateSO stationSO)
    {
        gameManager.mapGrid.AddStation(stationSO);
        gameObject.SetActive(false);
    }


}
