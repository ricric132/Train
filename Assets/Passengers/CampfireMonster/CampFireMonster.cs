using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampFireMonster : Passenger, IOnWarmGenEffect
{
    [SerializeField] GameObject unlitSprite;
    [SerializeField] GameObject litSprite;

    bool lit = false;

    public override void Start()
    {
        unlitSprite.SetActive(true);
        litSprite.SetActive(false);
        if (startRan)
        {
            return;
        }
        base.Start();
        triggerEffectHandler.AddEffect(gameObject);
    }

    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());

        if(lit)
        {
            /*
            List<Seat> adj = trainManager.GetNeighboringSeats(seat);
            for (int i = 0; i < adj.Count; i++)
            {
                if (adj[i].GetPassenger())
                {
                    adj[i].GetPassenger().Warm();
                }
            }
            */

            trainManager.GetTrainCar(seat).UpdateWarmAmt(1);
        }

        yield return null;
    }

    public IEnumerator OnWarmGen(TrainCar seat, int warmAmt)
    {
        Debug.Log("on fireee");
        if(lit == false)
        {
            lit = true;
            trainManager.GetTrainCar(this.seat).UpdateWarmAmt(warmAmt);
        }
        unlitSprite.SetActive(false);
        litSprite.SetActive(true);
        yield return null;
    }


    /*
    public override void Warm()
    {
        base.Warm();
        lit = true;
    }
    */
}
