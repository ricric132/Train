using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loner : Passenger
{
    int coinGainAmount = 5;
    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());

        List<Seat> adj = trainManager.GetNeighboringSeats(seat);
        bool empty = true;

        for (int i = 0; i < adj.Count; i++)
        {
            if(adj[i].GetPassenger() && adj[i].GetPassenger() != this)
            {
                empty = false;
            }
        }

        if (empty)
        {
            UpdateCoins(coinGainAmount);
        }
        yield return null;
    }
}
