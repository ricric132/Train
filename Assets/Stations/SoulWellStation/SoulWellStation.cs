using UnityEngine;

public class SoulWellStation : Station
{
    public SoulWellStation(BuildingTemplateSO _stationTemplate) : base(_stationTemplate)
    {

    }

    public override void Awake()
    {
        base.Awake();
        numPassengers = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPassengerDepart(Passenger passenger)
    {
        Debug.Log("spawn");
        Passenger curPassenger = passengerGenerator.GenerateCharacter();

        AddPassenger(curPassenger);

        UpdateQueueSpots();
    }
}
