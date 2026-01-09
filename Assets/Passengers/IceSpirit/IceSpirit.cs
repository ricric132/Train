using UnityEngine;

public class IceSpirit : Passenger
{
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);
        StartCoroutine(trainManager.DoEffectOnNeighbours(IncreaseCoin, seat));
    }

    public void IncreaseCoin(Passenger passenger)
    {
        if (passenger == null || passenger == this)
        {
            return;
        }
        passenger.Chill();
    }
}
