using UnityEngine;
using System.Collections;

public class Farmland : Building, IOnEnterStationEffect
{
    int skipTargets = 1;
    GameManager gameManager;
    TriggerEffectHandler triggerEffectHandler;
    TrainManager trainManager;

    [SerializeField] GameObject projectilePrefab;

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
        int passengerCount = trainManager.GetPassengerCount();

        if(passengerCount%2 == 1)
        {
            Debug.Log("Farmland Triggered");
            for (int i = 0; i < skipTargets; i++)
            {
                Passenger randPassenger = trainManager.GetRandomPassenger();

                GameObject projectile = Instantiate(projectilePrefab, GameManager.Instance.canvasManager.effectsCanvas.transform);
                yield return StartCoroutine(projectile.GetComponent<Projectile>().LaunchAndWait(transform.position, randPassenger.transform, 1));
                randPassenger.UpdateStationsRemaining(-1);
            }

        }
        yield return null;
    }

}
