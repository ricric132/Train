using UnityEngine;

public class FireSpirit : Passenger
{
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);
        //StartCoroutine(trainManager.DoEffectOnNeighbours(IncreaseCoin, seat));
        trainManager.GetTrainCar(_seat).UpdateWarmAmt(1);
    }

    /*
    public void IncreaseCoin(Passenger passenger)
    {
        if (passenger == null || passenger == this)
        {
            return;
        }
        passenger.Warm();
    }
    */
    
}
