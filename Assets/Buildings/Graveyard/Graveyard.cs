using UnityEngine;
using System.Collections;
using System.IO;

public class Graveyard : Building, IOnBoneGenEffect, IOnEnterStationEffect, IOffBoardEffect
{
    GameManager gameManager;
    TriggerEffectHandler triggerEffectHandler;
    TrainManager trainManager;

    [SerializeField] GameObject projectilePrefab;

    public int maxCharges = 1;
    int charges = 1;

    public override void Awake()
    {
        base.Awake();

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        trainManager = gameManager.trainManager;
        triggerEffectHandler = gameManager.triggerEffectHandler;
        triggerEffectHandler.AddEffect(gameObject);
    }

    public IEnumerator OnEnterStationTrigger()
    {
        charges = maxCharges;
        yield return null;
    }

    public IEnumerator OffBoardTrigger(Passenger p)
    {
        if (charges > 0)
        {
            GameObject projectile = Instantiate(projectilePrefab, GameManager.Instance.canvasManager.effectsCanvas.transform);
            yield return StartCoroutine(projectile.GetComponent<Projectile>().LaunchAndWait(transform.position, p.seat.transform, 1));
            p.seat.UpdateBones(1);
            charges--;
        }
        yield return null;
    }

    
    public IEnumerator OnBoneGen(TrainCar car, int boneAmt) { 
        /*
        if(charges > 0)
        {
            seat.UpdateBones(1);
            charges--;
        }
        */
        yield return null;
        
    }
    
}
