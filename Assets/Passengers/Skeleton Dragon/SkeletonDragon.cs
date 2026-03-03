using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class SkeletonDragon : Passenger
{
    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        TrainCar curCar = trainManager.GetTrainCar(_seat);

        while(curCar.GetBoneAmt()>=0)
        {
            curCar.UpdateBoneAmt(-1); 

            TrainCar frontCar = trainManager.GetAheadTrainCar(curCar);
            while (frontCar != null)
            {
                frontCar.UpdateWarmAmt(1);
                frontCar = trainManager.GetAheadTrainCar(frontCar);
            }

            TrainCar backCar = trainManager.GetAheadTrainCar(curCar);
            while (backCar != null)
            {
                backCar.UpdateChillAmt(1);
                backCar = trainManager.GetAheadTrainCar(frontCar);
            }
        }
       

        //for(int i = 0; i)
        /*
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
        */
    }
}
