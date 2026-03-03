using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using Unity.VisualScripting;

public class StationPath : MonoBehaviour
{
    public List<Station> path = new List<Station>();
    public int stationNumber;


    public PassengerGenerator passengerGenerator;

    [SerializeField] Transform stationParent;
    [SerializeField] GameObject upgradeStation;
    Station curStation;


    public List<Transform> queueSpots; 

    public CanvasManager canvasManager;

    public MapCanvas mapCanvas;

    public GameManager gameManager;

    public List<BuildingTemplateSO> randomStartingStationPool;

    int startingLength = 3; 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        canvasManager = gameManager.canvasManager;
        //GenerateRandomStartingPath();
        //GoToStation(0);
    }

    public void GenerateRandomStartingPath()
    {
        path.Clear();
        for (int i = 0; i < startingLength; i++)
        {
            AddStation(GetRandomStation());
        }
    }

    public BuildingTemplateSO GetRandomStation()
    {
        int options = randomStartingStationPool.Count;
        return randomStartingStationPool[UnityEngine.Random.Range(0, options)]; //add other pools later
    }

    public void RemovePrevStation()
    {
        if(curStation != null)
        {
            curStation.RemovePassengers();
            curStation.gameObject.SetActive(false);
        }

        upgradeStation.SetActive(false);
    }

   
    public void GenerateUpgradeStation()
    {
        upgradeStation.SetActive(true);
    }

    public void GenerateStation(Station station)
    {
        /*
        if (curStation != null)
        {
            curStation.RemovePassengers();
        }
        */

        curStation = station;
        curStation.gameObject.SetActive(true);
        curStation.GeneratePassengers();
    }

    public Station AddStation(BuildingTemplateSO stationTemplate)
    {
        GameObject newStation = Instantiate(stationTemplate.prefab, stationParent);
        newStation.SetActive(false);
        path.Add(newStation.GetComponent<Station>());
        mapCanvas.UpdateQueueVisual(path);
        return newStation.GetComponent<Station>();
    }

    public Station GetCurStation()
    {
        return curStation;
    }

    /* ARCHIVED from old system
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
    
    
    public void GoToStation(int num)
    {
        stationNumber = num;
        canvasManager.SetupStationsPreview(path, stationNumber);
        GenerateStation();
    }

    public void GenerateStation()
    {
        if (curStation != null)
        {
            curStation.RemovePassengers();
        }

        curStation = path[stationNumber];
        curStation.gameObject.SetActive(true);
        curStation.GeneratePassengers();
    }

    public void GenerateRandomPath()
    {
        path.Clear();
        int length = UnityEngine.Random.Range(5, 10);

        for (int i = 0; i < length; i++)
        {
            path.Add((Station.Types)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Station.Types)).Cast<Station.Types>().Max()));
        }
    }

    public bool NextStation()
    {
        stationNumber++;
        if(stationNumber >= path.Count)
        {
            stationNumber = -1;
            //gameManager.LoopComplete();
            return false;
            //GenerateRandomPath();
        }


        canvasManager.SetupStationsPreview(path, stationNumber);

        return true;
    }
    */
}

public class StationPoolParams
{
    float basePool = 1;
}
