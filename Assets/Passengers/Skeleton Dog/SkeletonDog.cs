using UnityEngine;

public class SkeletonDog : Passenger
{
    int skipAmt = 2;
    int coinIncrease = 5;
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);
        if(_seat.GetBones() >= 1)
        {
            seat.UpdateBones(-1);
            UpdateCoins(coinIncrease);
            UpdateStationsRemaining(skipAmt);

        }
    }

}
