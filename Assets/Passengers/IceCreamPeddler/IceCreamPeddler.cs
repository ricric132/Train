using UnityEngine;

public class IceCreamPeddler : Passenger
{
    int increaseAmt= 1;

    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        for (int i = 0; i < trainManager.GetTrainCar(_seat).GetChillAmt() + 1; i++)
        {
            Passenger target = trainManager.GetRandomOtherPassenger(this);
            target.UpdateCoins(increaseAmt);
        }

    }

    /*
    public override void Chill()
    {
        base.Chill();

        for (int i = 0; i < iceCreamAmount; i++)
        {
            Passenger target = trainManager.GetRandomOtherPassenger(this);
            target.UpdateCoins(increaseAmt);
        }
    }
    */
}
