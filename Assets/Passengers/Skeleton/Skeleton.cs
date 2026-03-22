using UnityEngine;

public class Skeleton : Passenger
{
    int boneCount = 2;
    public override void OnDepart()
    {
        base.OnDepart();
        trainManager.GetTrainCar(seat).UpdateBoneAmt(1);
    }
}
