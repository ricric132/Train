using TMPro;
using UnityEngine;

public class TrainCar : MonoBehaviour 
{
    [SerializeField]CarBuffIndicator carBuffIndicator;

    int boneAmt = 0;
    int chillAmt = 0;
    int warmAmt = 0;

    void Update()
    {
        carBuffIndicator.UpdateText(boneAmt, chillAmt, warmAmt);
    }

    public int GetBoneAmt()
    {
        return boneAmt;
    }

    public int GetChillAmt() 
    { 
        return chillAmt;
    }

    public int GetWarmAmt()
    {
        return warmAmt;
    }

    public void SetBoneAmt(int amt)
    {
        boneAmt = amt;
    }

    public void SetChillAmt(int amt)
    {
        chillAmt = amt;
    }

    public void SetWarmAmt(int amt)
    {
        warmAmt = amt;
    }

    public void UpdateBoneAmt(int amt)
    {
        StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerBoneGen(this, amt));
        boneAmt += amt;
    }

    public void UpdateChillAmt(int amt)
    {
        StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerChillGen(this, amt));
        chillAmt += amt;
    }

    public void UpdateWarmAmt(int amt)
    {
        StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerWarmGen(this, amt));
        warmAmt += amt;
    }

}
