using UnityEngine;
using System.Collections;

public class Departer : Passenger, IOffBoardEffect
{
    public int offboardBonus = 1;

    public override void Start()
    {
        if (startRan)
        {
            return;
        }
        base.Start();
        //Debug.Log("ADDED");
        triggerEffectHandler.AddEffect(gameObject);
    }


    public IEnumerator OffBoardTrigger(Passenger p)
    {
        //Debug.Log("triggered depart");
        if (p.ReachedStation())
        {
            UpdateCoins(2 * offboardBonus);
        }
        else
        {
            UpdateCoins(offboardBonus);
        }
        yield return null;
    }

    private void OnDisable()
    {
        triggerEffectHandler.RemoveEffect(gameObject);
    }

}
