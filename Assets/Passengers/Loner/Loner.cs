using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loner : Passenger
{
    int coinGainAmount = 5;
    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());

        List<Seat> sameCarSeats = trainManager.GetSameCarSeats(seat);

        bool empty = true;
        for (int i = 0; i < sameCarSeats.Count; i++)
        {
            if(sameCarSeats[i].GetPassenger() && sameCarSeats[i].GetPassenger() != this)
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
