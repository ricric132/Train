using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseBudget", menuName = "ScriptableObjects/ContractEffects/IncreaseBudget")]
public class IncreaseBudget : ContractEffect
{
    public float increaseAmount;

    public IncreaseBudget() : base()
    {
        description = "Increases budget by " + (increaseAmount * 100) + "% for next day";
    }

    public override void EnableEffect(GameManager gm)
    {
        gm.UpdateCurrencyMult(increaseAmount);
        
    }

    public override void RevertEffect(GameManager gm)
    {
        gm.UpdateCurrencyMult(-increaseAmount);
    }
}

