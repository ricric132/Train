using UnityEngine;

public class Skeleton : Passenger
{
    int boneCount = 1;
    public override void OnDepart()
    {
        base.OnDepart();
        seat.UpdateBones(1);
    }
}
