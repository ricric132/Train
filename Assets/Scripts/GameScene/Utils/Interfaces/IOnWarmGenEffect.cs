using System.Collections;
using UnityEngine;

public interface IOnWarmGenEffect
{
    public IEnumerator OnWarmGen(TrainCar seat, int warmAmt);
}
