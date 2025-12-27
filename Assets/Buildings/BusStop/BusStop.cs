using UnityEngine;

public class BusStop : Building, IStationBuff
{
    public override void Awake()
    {
        base.Awake();
        stationEffectRange = 1;
    }

    public override void Setup()
    {
        base.Setup();

        for (int i = -stationEffectRange; i <= stationEffectRange; i++)
        {
            for (int j = -stationEffectRange; j <= stationEffectRange; j++)
            {
                Station station = map.grid[coords.x + i, coords.y + j].GetStation();
                if (station != null)
                {
                    station.stationBuffs.Add(this);
                }
            }
        }
    }

    public override void Remove()
    {
        base.Remove();
    }

    public void PreGenBuff(Station station)
    {

    }
    public void PostGenBuff(Station station)
    {
        Passenger extraPassenger = station.passengerGenerator.GenerateCharacterFromPool();
        station.AddPassenger(extraPassenger);
    }
    public void MidGenBuff(Station station)
    {

    }
}
