using System.Collections.Generic;
using UnityEngine;


public class StationSelection : MonoBehaviour
{
    [SerializeField] List<StationChoice> stationSelectionSlots;

    [SerializeField] List<BuildingTemplateSO> allStationTemplates;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeStationSelect()
    {
        foreach(StationChoice station in stationSelectionSlots)
        {
            station.stationSO = allStationTemplates[Random.Range(0, allStationTemplates.Count)];
        }
    }




}
