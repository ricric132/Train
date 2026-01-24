using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Hydra : Passenger
{
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        if (seat.seatOrder == Seat.SeatOrder.Front)
        {
            UpdateCoins(10);
            UpdateStationsRemaining(-2);
        }
        
        if(seat.seatOrder == Seat.SeatOrder.Back)
        {
            List<Seat> adj = trainManager.GetNeighboringSeats(seat);
            if (adj.Count > 0)
            {
                Eat(adj[0].GetPassenger());
            }
        }
    }

    public override IEnumerator NextStationAction()
    {
        base.NextStationAction();
        if(seat.seatOrder == Seat.SeatOrder.Mid)
        {
            Seat frontSeat = trainManager.GetSeatAhead(seat);

            if (frontSeat != null && frontSeat.GetPassenger())
            {
                frontSeat.GetPassenger().UpdateCoins(3);
                frontSeat.GetPassenger().UpdateStationsRemaining(-1);
            }
        }

        yield return null;
    }

    public void Eat(Passenger p)
    {
        if(p == null) 
        {
            return;
        }

        //Debug.Log("NOM");
        int coinsAmt = p.info.coins;

        p.Remove();

        UpdateCoins(coinsAmt/2);
    }
}
