using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CampFireMonster : Passenger, IOnWarmGenEffect
{
    [SerializeField] Sprite unlitSprite;
    [SerializeField] Sprite litSprite;

    bool lit = false;

    public override void Start()
    {
        visual.GetComponent<SpriteRenderer>().sprite = unlitSprite;
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
        visual.GetComponent<SpriteRenderer>().sprite = litSprite;
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
