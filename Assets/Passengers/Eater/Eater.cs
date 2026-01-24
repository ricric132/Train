using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Eater : Passenger
{
    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());


        yield return new WaitForSeconds(1f/gameManager.animationSimSpeed);
            
        List<Seat> adj = trainManager.GetNeighboringSeats(seat);
        List<Passenger> adjP = new List<Passenger>();
        for (int i = 0; i < adj.Count; i++)
        {
            if (adj[i].GetPassenger() != null && adj[i].GetPassenger() is not StationaryItem)
            {
                adjP.Add(adj[i].GetPassenger());
            }
        }

        Debug.Log("passenger adj: " + adjP.Count);

        if (adjP.Count > 0)
        {
            Eat(adjP[UnityEngine.Random.Range(0, adjP.Count)]);
        }
        
    }

    public void Eat(Passenger p)
    {
        //Debug.Log("NOM");
        int stationAmt = p.info.stopsRemaining;
        int coinsAmt = p.info.coins;

        p.Remove();

        UpdateCoins(coinsAmt);
        UpdateStationsRemaining(stationAmt);

    }
}
