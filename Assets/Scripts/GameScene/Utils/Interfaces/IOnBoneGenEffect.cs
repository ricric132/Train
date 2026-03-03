using System.Collections;
using UnityEngine;

public interface IOnBoneGenEffect
{
    public IEnumerator OnBoneGen(TrainCar seat, int boneAmt);
}
