using UnityEngine;
using System.Collections;

public class TimeWarper : Passenger
{
    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());
        yield return new WaitForSeconds(1f);

        Passenger p = trainManager.GetRandomPassenger();
        StartCoroutine(p.NextStationAction());
        yield return null;
    }
}
