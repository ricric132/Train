using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Passenger
{
    [SerializeField] SpeciesSO boneConstructSO;

    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);

        List<Seat> sameCarSeats = trainManager.GetSameCarSeats(_seat);
        int boneCount = 0;

        foreach (Seat s in sameCarSeats)
        {
            boneCount += s.GetBones();
            s.UpdateBones(-s.GetBones());
        }

        //maybe have diff bone constructs with diff effects with certain thresholds of bones
        List<Seat> adj = trainManager.GetNeighboringSeats(_seat);
        List<Seat> emptySeats = new List<Seat>();
        for(int i = 0; i < adj.Count; i++)
        {
            if (adj[i].occupiedGO == null)
            {
                emptySeats.Add(adj[i]);
            }
        }

        if(emptySeats.Count == 0)
        {
            return; // no space
        }

        Passenger summon = passengerGenerator.GenerateCharacter(boneConstructSO);
        summon.station = station;

        Seat randSeat = emptySeats[UnityEngine.Random.Range(0, emptySeats.Count)];

        summon.transform.parent = randSeat.transform;
        summon.transform.localPosition = Vector3.zero;
        randSeat.occupiedGO = summon.gameObject;
        summon.ManualStart();
        summon.UpdateCoins(boneCount * 5);
    }
}
