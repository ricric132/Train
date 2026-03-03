using System.Collections;
using UnityEngine;

public interface IOnChillGenEffect
{
    public IEnumerator OnChillGen(TrainCar seat, int warmAmt);
}
