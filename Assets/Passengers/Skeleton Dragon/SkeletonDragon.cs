using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class SkeletonDragon : Passenger
{
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        int seatNum = trainManager.seats.IndexOf(_seat);
        List<Passenger> ahead = trainManager.GetAllPassengers(0, seatNum);
        List<Passenger> behind = trainManager.GetAllPassengers(start: seatNum + 1);

        for (int i = 0; i < _seat.GetBones(); i++)
        {
            //Debug.Log("Buffed");
            Passenger randAhead = ahead[Random.Range(0, ahead.Count)];
            Passenger randBehind = behind[Random.Range(0, behind.Count)];

            randAhead.Warm();
            randBehind.Chill();
        }
    }
}
