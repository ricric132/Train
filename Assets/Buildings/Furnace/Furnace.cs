using System.Collections;
using UnityEngine;

public class Furnace : Building, IOnBoardEffect
{
    public PlayerManager playerManager;
    TriggerEffectHandler triggerEffectHandler;
    int addAmount = 1;

    public override void Awake()
    {
        base.Awake();
        playerManager = FindFirstObjectByType<PlayerManager>();
        triggerEffectHandler = FindFirstObjectByType<TriggerEffectHandler>();
        triggerEffectHandler.AddEffect(gameObject);
    }
    public bool OnBoardCheckTrigger(Passenger p)
    {
        //Debug.Log("checkk");
        return true;
    }

    public IEnumerator OnBoardTrigger(Passenger p)
    {
        //Debug.Log("trigggerrrs");
        playerManager.AddPositive(addAmount);
        yield return null;
    }
}
