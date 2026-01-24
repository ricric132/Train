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
        gameManager = GameManager.Instance;

        for(int i = 0; i < seats.Count; i++)
        {
            seats[i].seatOrder = (Seat.SeatOrder)(i % 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopped)
        {
            trainIcon.Move(gameManager.trainMoveSimSpeed);
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
            trainFront.position = new Vector3(trainFront.position.x, driveSpeed * Time.deltaTime * gameManager.animationSimSpeed + trainFront.position.y, trainFront.position.z);
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
            trainFront.position = new Vector3(trainFront.position.x, driveSpeed * Time.deltaTime * gameManager.animationSimSpeed + trainFront.position.y, trainFront.position.z);
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
                Debug.Log("null at - " + i);
                continue;
            }
            cameraManager.PanTo(toSim[i].gameObject, gameManager.animationSimSpeed);
            yield return new WaitForSeconds(0.5f/gameManager.animationSimSpeed);
            yield return StartCoroutine(curStation.EffectOnPassenger(toSim[i]));
            yield return StartCoroutine(toSim[i].NextStation());
        }

        cameraManager.ResetToDefault();

        yield return new WaitForSeconds(1);
    }

    public bool HasDisembarkable()
    {
        for (int i = 0; i < seats.Count; i++)
        {
            Passenger p = seats[i].GetPassenger();
            if (p && p.ReachedStation())
            {
                return true;
            }
        }
        return false;
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

    public Seat GetSeatAhead(Seat seat, bool sameCar = true)
    {
        int seatIndex = seats.IndexOf(seat);

        if ((sameCar && seatIndex%3 == 2) || seatIndex == seats.Count)
        {
            return null;
        }
        return seats[seatIndex + 1];
    }
    public Seat GetSeatBehind(Seat seat, bool sameCar = true)
    {
        int seatIndex = seats.IndexOf(seat);

        if ((sameCar && seatIndex % 3 == 0) || seatIndex == 0)
        {
            return null;
        }
        return seats[seatIndex - 1];
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
        List<Passenger> allPassengers = GetAllPassengers();

        return allPassengers[UnityEngine.Random.Range(0, allPassengers.Count)];
    }

    public Passenger GetRandomOtherPassenger(Passenger p)
    {
        List<Passenger> allPassengers = GetAllOtherPassengers(p);

        return allPassengers[UnityEngine.Random.Range(0, allPassengers.Count)];
    }

    public List<Passenger> GetAllPassengers(int start = 0, int end = 0, bool includeItems = false)
    {
        List<Passenger> allPassengers = new List<Passenger>();
        int lastIndex = seats.Count;
        if(end > 0)
        {
            lastIndex = Mathf.Min(lastIndex, end);
        }

        for (int i = start; i < lastIndex; i++)
        {
            Passenger p = seats[i].GetPassenger();
            if (p != null && p is not StationaryItem || includeItems == true)
            {
                allPassengers.Add(seats[i].GetPassenger());

            }
        }

        return allPassengers;
    }

    public List<Passenger> GetAllOtherPassengers(Passenger p, int start = 0, int end = 0)
    {
        List<Passenger> allPassengers = GetAllPassengers(start, end);

        allPassengers.Remove(p);

        return allPassengers;
    }

    public void AddPassenger(Passenger passenger, Seat seat)
    {
        passenger.HandleSnap(seat);
    }

    public List<Seat> GetActiveSeats()
    {
        List<Seat> activeSeats = new List<Seat>();
        for (int i = 0; i < seats.Count; i++)
        {
            if (seats[i].CheckActive())
            {
                activeSeats.Add(seats[i]);
            }
        }
        return activeSeats;
    }

    public int GetPassengerCount()
    {
        int count = 0;
        for (int i = 0; i < seats.Count; i++)
        {
            if (seats[i].occupiedGO != null)
            {
                count++;
            }
        }
        return count;
    }

    public void ContractDisableSeats(int amount, int days)
    {
        List<Seat> activeSeats = GetActiveSeats();

        for (int i = 0; i < amount; i++)
        {
            Seat toBeDisabled = activeSeats[UnityEngine.Random.Range(0, activeSeats.Count)];
            if (toBeDisabled != null)
            {
                toBeDisabled.contractDisableDuration += days;
            }
        }
    }

    public void DecrimentContractDays()
    {
        for (int i = 0; i < seats.Count; i++)
        {
            if(seats[i].contractDisableDuration > 0)
            {
                seats[i].contractDisableDuration -= 1;
            }
        }
    }

}
