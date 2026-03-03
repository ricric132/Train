using UnityEngine;
using System.Collections.Generic;

public class Kraken : Passenger
{
    public List<SpeciesSO> spawnableObjects;

    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        List<Seat> adj = trainManager.GetNeighboringSeats(seat);

        for (int i = 0; i < adj.Count; i++)
        {
            if (adj[i].GetPassenger() == null && adj[i].CheckActive())
            {
                Passenger spawn = passengerGenerator.GenerateCharacter(spawnableObjects[Random.Range(0, spawnableObjects.Count)]);
                trainManager.AddPassenger(spawn, adj[i]);
            }
        }
    }

}
