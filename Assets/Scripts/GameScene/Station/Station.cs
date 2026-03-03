using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Station : MonoBehaviour
{
    public enum Types
    {
        Red,
        Green,
        Blue,
        Black
    }

    public BuildingTemplateSO stationTemplate;

    public List<Passenger> saved;

    public List<Passenger> inQueue;

    public List<Transform> queueSpots;



    public float spacing = 2;

    public Types type;

    public int numPassengers = 3;

    public PassengerGenerator passengerGenerator;
    public TrainManager trainManager;
    public CameraManager cameraManager;
    
    public List<IStationBuff> stationBuffs;


    public virtual void Awake()
    {
        stationBuffs = new List<IStationBuff>();
        passengerGenerator = FindFirstObjectByType<PassengerGenerator>();
        trainManager = FindFirstObjectByType<TrainManager>();
        cameraManager = FindFirstObjectByType<CameraManager>();
    }

    public virtual IEnumerator StationEnter()
    {
        yield return null;
    }

    public virtual IEnumerator EffectOnPassenger(Passenger passenger)
    {
        yield return null;
    }

    public virtual void GeneratePassengers()
    {
        for (int i = 0; i < stationBuffs.Count; i++)
        {
            stationBuffs[i].PreGenBuff(this);
        }
        List<Passenger> passengerList = new List<Passenger>();

        for (int i = 0; i < numPassengers; ++i)
        {
            Passenger curPassenger = passengerGenerator.GenerateCharacterFromPool();
            curPassenger.station = this;
            passengerList.Add(curPassenger);
        }

        inQueue = passengerList;
        UpdateQueueSpots();

        for(int i = 0;i < stationBuffs.Count; i++)
        {
            stationBuffs[i].PostGenBuff(this);
        }
    }

    public virtual void AddPassenger(Passenger p)
    {
        p.station = this;
        inQueue.Add(p);
    }

    public virtual Passenger AddPassengerFromPool()
    {
        SnappingPoint emptySpot = GetEmptyQueueSpot();

        if (emptySpot == null)
        {
            Debug.Log("not enough space");
            return null;
        }

        Passenger p = passengerGenerator.GenerateCharacterFromPool();
        p.station = this;
        inQueue.Add(p);

        p.queueSpot = emptySpot.transform;
        p.transform.parent = emptySpot.transform;
        p.transform.localPosition = Vector3.zero;
        emptySpot.occupiedGO = p.gameObject;

        Debug.Log("spawn success");

        return p;
    }

    public virtual Passenger AddPassenger(SpeciesSO species)
    {
        SnappingPoint emptySpot = GetEmptyQueueSpot();

        if(emptySpot == null)
        {
            Debug.Log("not enough space");
            return null;
        }

        Passenger p = passengerGenerator.GenerateCharacter(species);
        p.station = this;
        inQueue.Add(p);


        p.queueSpot = emptySpot.transform;
        p.transform.parent = emptySpot.transform;
        p.transform.localPosition = Vector3.zero;
        emptySpot.occupiedGO = p.gameObject;

        Debug.Log("spawn success");

        return p;
    }

    public virtual void OnPassengerDepart(Passenger passenger)
    {

    }

    public void RemoveFromQueue(Passenger passenger)
    {
        if (inQueue.Contains(passenger))
        {
            inQueue.Remove(passenger);
        }
    }

    public virtual void RemovePassengers()
    {
        Debug.Log("removed passengers at " + stationTemplate.buildingName);
        for(int i = 0; i < inQueue.Count; i++)
        {
            if (inQueue[i].seat == null)
            {
                
                inQueue[i].Remove();
                
            }
        }

        inQueue.Clear();
    }

    public virtual void UpdateQueueSpots()
    {
        for (int i = 0; i < inQueue.Count; i++)
        {
            queueSpots[i].GetComponent<SnappingPoint>().occupiedGO = inQueue[i].gameObject;
            inQueue[i].queueSpot = queueSpots[i];
            inQueue[i].transform.parent = queueSpots[i];
            inQueue[i].transform.localPosition = Vector3.zero;
        }
    }

    SnappingPoint GetEmptyQueueSpot()
    {
        for (int i = 0; i < queueSpots.Count; i++)
        {
            if (queueSpots[i].GetComponent<SnappingPoint>().occupiedGO == null)
            {
                return queueSpots[i].GetComponent<SnappingPoint>();
            }
        }

        return null;
    }


}
