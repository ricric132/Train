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
 
    }

    public override void Start()
    {
        base.Start();
        playerManager = GameManager.Instance.playerManager;
        triggerEffectHandler = GameManager.Instance.triggerEffectHandler;
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
