using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseQuota", menuName = "ScriptableObjects/ContractEffects/IncreaseQuota")]
public class IncreaseQuota : ContractEffect
{
    public float increaseAmount;

    public IncreaseQuota() : base()
    {
        description = "Increases quota by " + (increaseAmount * 100) + "% for next day";
    }

    public override void EnableEffect()
    {
        GameManager.Instance.UpdateQuotaMult(increaseAmount);
    }

    public override void RevertEffect()
    {
        GameManager.Instance.UpdateQuotaMult(-increaseAmount);    

    }
}
