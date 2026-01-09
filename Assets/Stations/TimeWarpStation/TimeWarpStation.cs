using UnityEngine;
using System.Collections;

public class TimeWarpStation : Station
{
    int stationsSkipped; 


    public override void Awake()
    {
        base.Awake();
        numPassengers = 3;
        stationsSkipped = 1;
    }

    public override IEnumerator StationEnter()
    {
        Debug.Log("Enter");
        for (int i = 0; i < trainManager.seats.Count; i++)
        {
            if (trainManager.seats[i].occupiedGO != null)
            {
                //cameraManager.PanTo(trainManager.seats[i].gameObject);
                trainManager.seats[i].GetPassenger().UpdateStationsRemaining(-stationsSkipped);
                //yield return new WaitForSeconds(1);
            }
        }
        yield return null;
    }

    public override void GeneratePassengers()
    {
        base.GeneratePassengers();
    }

    public override void RemovePassengers()
    {
        base.RemovePassengers();
    }

    public override void UpdateQueueSpots()
    {
        base.UpdateQueueSpots();
    }

}
