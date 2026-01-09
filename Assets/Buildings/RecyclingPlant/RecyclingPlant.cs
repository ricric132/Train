using UnityEngine;
using System.Collections;
using TMPro;

public class RecyclingPlant : Building, IOffBoardEffect, IOnEnterStationEffect
{
    int counter = 0;
    int requiredAmount = 3;
    PlayerManager playerManager;
    TriggerEffectHandler triggerEffectHandler;
    [SerializeField] TextMeshProUGUI numIndicator;

    public override void Start()
    {
        base.Start();
        playerManager = GameManager.Instance.playerManager;
        triggerEffectHandler = GameManager.Instance.triggerEffectHandler;
        triggerEffectHandler.AddEffect(gameObject);
        UpdateText();
    }

    public IEnumerator OnEnterStationTrigger()
    {
        counter = 0;

        yield return null;
    }

    public IEnumerator OffBoardTrigger(Passenger p)
    {
        counter++;
        UpdateText();
        if (counter >= requiredAmount)
        {
            GameManager.Instance.playerManager.AddNegative(-1);
            counter = 0;
            Debug.Log("Recycle triggered");
            UpdateText();
        }

        yield return null;
    }

    void UpdateText()
    {
        numIndicator.text = counter.ToString() + "/" + requiredAmount.ToString(); 
    }
}
