using UnityEngine;

public class IceCreamPeddler : Passenger
{
    int iceCreamAmount = 2;
    int increaseAmt= 1;

    public override void Chill()
    {
        base.Chill();

        for (int i = 0; i < iceCreamAmount; i++)
        {
            Passenger target = trainManager.GetRandomOtherPassenger(this);
            target.UpdateCoins(increaseAmt);
        }
    }
}
