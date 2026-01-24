using UnityEngine;

public class Skeleton : Passenger
{
    int boneCount = 2;
    public override void OnDepart()
    {
        base.OnDepart();
        seat.UpdateBones(1);
    }
}
