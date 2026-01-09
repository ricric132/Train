using UnityEngine;
using System.Collections;

public class Departer : Passenger, IOffBoardEffect
{
    public int offboardBonus = 1;

    public override void Start()
    {
        base.Start();
        triggerEffectHandler.AddEffect(gameObject);
    }


    public IEnumerator OffBoardTrigger(Passenger p)
    {
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
