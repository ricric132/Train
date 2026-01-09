using UnityEngine;
using System.Collections;

public class FireStation : Station
{
    int addedFireSpirits;
    [SerializeField] SpeciesSO fireSpiritSO;


    public override void Awake()
    {
        base.Awake();
        numPassengers = 3;
        addedFireSpirits = 2;
    }

    public override IEnumerator StationEnter()
    {
        yield return null;
    }

    public override void GeneratePassengers()
    {
        base.GeneratePassengers();
        for(int i = 0; i < addedFireSpirits; i++)
        {
            Passenger p = passengerGenerator.GenerateCharacter(fireSpiritSO);
            p.station = this;
            inQueue.Add(p);
        }

        UpdateQueueSpots();
    }

    public override void RemovePassengers()
    {
        base.RemovePassengers();
    }

    public override void UpdateQueueSpots()
    {
        base.UpdateQueueSpots();
    }

}
