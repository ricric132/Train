using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting;
using UnityEngine;

public class SmugglersStation : Station
{
    public SmugglersStation(BuildingTemplateSO _stationTemplate) : base(_stationTemplate)
    {

    }

    public override void Awake()
    {
        base.Awake();
        numPassengers = 3;
    }
    public override void GeneratePassengers()
    {
        //do nothing
    }
    public override IEnumerator StationEnter()
    {
        List<Seat> emptySeats = new List<Seat>();
        for(int i = 0; i < trainManager.seats.Count; i++)
        {
            if (trainManager.seats[i].occupiedGO == null)
            {
                emptySeats.Add(trainManager.seats[i]);
            }
        }

        for (int i = 0; i < numPassengers && emptySeats.Count > 0; ++i)
        {
            Passenger curPassenger = passengerGenerator.GenerateCharacterFromPool();
            curPassenger.station = this;

            Seat randSeat = emptySeats[UnityEngine.Random.Range(0, emptySeats.Count)];

            curPassenger.transform.parent = randSeat.transform;
            curPassenger.transform.localPosition = Vector3.zero;
            randSeat.occupiedGO = curPassenger.gameObject;
            curPassenger.ManualStart();
            curPassenger.UpdateCoins(curPassenger.info.coins);

            curPassenger.OnSeated(randSeat);

            emptySeats.Remove(randSeat);
        }

        yield return null;
    }
}
