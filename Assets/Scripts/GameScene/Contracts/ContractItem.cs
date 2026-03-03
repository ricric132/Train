using TMPro;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class ContractItem : MonoBehaviour
{
    public Contract contract;
    [SerializeField] ContractManager contractManager;
    [SerializeField] TextMeshProUGUI negText;
    [SerializeField] TextMeshProUGUI posText;
    bool activated = false; 
    [SerializeField] Toggle toggle;

    public void Setup(Contract _contract)
    {
        contract = _contract;
        negText.text = contract.negative.description;
        posText.text = contract.positive.description;
        toggle.isOn = false;
    }

    private void Update()
    {
        if (toggle.isOn && activated == false)
        {
            activated = true;

        }
        else if (!toggle.isOn && activated == true)
        {
            activated = false; 
        }
    }

    public void CheckActivate()
    {
        if (activated)
        {
            contractManager.ActivateContract(contract);
        }
    }

    public float GetQuotaIncrease()
    {
        float increase = 0;
        if (!activated)
        {
            return 0;
        }

        if(contract.negative is IncreaseQuota increaseQuota)
        {
            increase += increaseQuota.increaseAmount;
        }

        return increase;
    }

    public float GetBudgetIncrease()
    {
        float increase = 0;
        if (!activated)
        {
            return 0;
        }

        if (contract.positive is IncreaseBudget increaseBudget)
        {
            increase += increaseBudget.increaseAmount;
        }

        return increase;
    }
}
