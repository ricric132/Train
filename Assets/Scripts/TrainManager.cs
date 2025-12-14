using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class TrainManager : MonoBehaviour
{
    public List<Seat> seats; 

    [SerializeField] CameraManager cameraManager;

    [SerializeField] StationPath stationPathManager;
    [SerializeField] GameManager gameManager;

    [SerializeField] Transform rightStop;
    [SerializeField] Transform stationStop;
    [SerializeField] Transform leftStop;

    [SerializeField] Transform trainFront;
    [SerializeField] float driveSpeed;

    MapTrain trainIcon;
    public bool stopped = false;

    Station prevStation = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trainIcon = FindFirstObjectByType<MapTrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            trainIcon.Move();
            Station curStation = trainIcon.CheckStation();
            if (curStation != null && curStation != prevStation) 
            {
                stopped = true;
                StartCoroutine(gameManager.EnterStation(curStation));
            }
            prevStation = curStation;
        }
    }



    public IEnumerator SimulateLeaveStation()
    {
        Debug.Log("GOOO");
        while (Mathf.Abs(trainFront.position.y - rightStop.position.y) > 0.1 && trainFront.position.y < rightStop.position.y)
        {
            trainFront.position = new Vector3(trainFront.position.x, driveSpeed * Time.deltaTime + trainFront.position.y, trainFront.position.z);
            yield return new WaitForEndOfFrame();
        }
        stopped = false;
        yield return null;
    }

    public IEnumerator SimulateEnterStation()
    {
        trainFront.position = new Vector2(trainFront.position.x, leftStop.position.y);

        while (Mathf.Abs(trainFront.position.y - stationStop.position.y) > 0.1 && trainFront.position.y < stationStop.position.y)
        {
            trainFront.position = new Vector3(trainFront.position.x, driveSpeed * Time.deltaTime + trainFront.position.y, trainFront.position.z);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public IEnumerator SimulatePassengerEffects(Station curStation)
    {
        List<Passenger> toSim = new List<Passenger>();
        for(int i = 0; i < seats.Count; i++)
        {
            if (seats[i].occupiedGO != null)
            {
                toSim.Add(seats[i].GetPassenger());

            }
        }

        for(int i = 0; i < toSim.Count; i++)
        {
            if (toSim[i] == null)
            {
                continue;
            }
            cameraManager.PanTo(toSim[i].gameObject);
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(curStation.EffectOnPassenger(toSim[i]));
            yield return StartCoroutine(toSim[i].NextStation());
        }

        cameraManager.ResetToDefault();

        yield return new WaitForSeconds(1);
    }

    public IEnumerator DoEffectOnAllPassengers(Action<Passenger> effect)
    {
        for (int i = 0; i < seats.Count; i++)
        {
            if (seats[i].occupiedGO != null)
            {
                //cameraManager.PanTo(seats[i].gameObject);
                effect(seats[i].GetPassenger());
                //yield return new WaitForSeconds(1);
            }
        }

        //cameraManager.ResetToDefault();

        yield return new WaitForSeconds(1);
    }

    public IEnumerator DoEffectOnNeighbours(Action<Passenger> effect, Seat seat)
    {
        List<Seat> adj = GetNeighboringSeats(seat);
        for(int i = 0; i < adj.Count; i++)
        {
            effect(adj[i].GetPassenger());
        }

        yield return null; 
    }

    public List<Seat> GetNeighboringSeats(Seat seat)
    {
        List<Seat> adj = new List<Seat>();  

        int seatIndex = seats.IndexOf(seat);
        int left = seatIndex - 1;
        int right = seatIndex + 1;

        if (left / 3 == seatIndex / 3 && left >= 0)
        {
            adj.Add(seats[left]);
        }


        if (right / 3 == seatIndex / 3 && right < seats.Count)
        {
            adj.Add(seats[right]);
        }

        return adj;
    }
    
    public List<Seat> GetSameCarSeats(Seat seat)
    {
        List<Seat> res = new List<Seat>();

        int seatIndex = seats.IndexOf(seat);

        int car = seatIndex / 3;
        for (int i = 0; i < 3; i++)
        {
            res.Add(seats[car * 3 + i]);
        }

        return res;
    }

    public Passenger GetRandomPassenger()
    {
        List<Passenger> allPassengers = new List<Passenger>();

        for (int i = 0; i < seats.Count; i++)
        {
            if (seats[i].occupiedGO != null)
            {
                allPassengers.Add(seats[i].GetPassenger());
            }
        }

        return allPassengers[UnityEngine.Random.Range(0, allPassengers.Count)];
    }

    public void AddPassenger(Passenger passenger, Seat seat)
    {
        passenger.HandleSnap(seat);
    }
}
