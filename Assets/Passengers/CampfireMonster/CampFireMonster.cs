using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampFireMonster : Passenger
{
    bool lit = false;

    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());

        if(lit)
        {
            List<Seat> adj = trainManager.GetNeighboringSeats(seat);
            for (int i = 0; i < adj.Count; i++)
            {
                if (adj[i].GetPassenger())
                {
                    adj[i].GetPassenger().Warm();
                }
            }
        }

        yield return null;
    }
    public override void Warm()
    {
        base.Warm();
        lit = true;
    }
}
