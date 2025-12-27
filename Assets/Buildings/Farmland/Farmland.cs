using UnityEngine;

public class Farmland : Building, IOnEnterStationEffect
{
    int skipTargets = 1;
    TriggerEffectHandler triggerEffectHandler;

    public override void Awake()
    {
        base.Awake();
        triggerEffectHandler = FindFirstObjectByType<TriggerEffectHandler>();
        triggerEffectHandler.AddEffect(gameObject);
    }

    public IEnumerator OnEnterStationTrigger()
    {

    }

}
