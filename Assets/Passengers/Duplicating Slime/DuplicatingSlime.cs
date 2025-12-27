using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DuplicatingSlime : Passenger
{
    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());


        if(Random.value > 0.5)
        {
            yield return new WaitForSeconds(1f / gameManager.animationSimSpeed);

            List<Seat> adj = trainManager.GetNeighboringSeats(seat);

            for (int i = 0; i < adj.Count; i++)
            {
                if (adj[i].GetPassenger() == null)
                {
                    Passenger dupe = passengerGenerator.GenerateCharacter(info.species);
                    info.CopyTo(dupe.info);
                    trainManager.AddPassenger(dupe, adj[i]);
                    break;
                }
            }
        }   
    }
}
