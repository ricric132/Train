using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ContractManager : MonoBehaviour
{
    [SerializeField] List<ContractEffect> allNegativeContractsEffects = new List<ContractEffect>();
    [SerializeField] List<ContractEffect> allPositiveContractsEffects = new List<ContractEffect>();

    public List<Contract> allActiveContracts = new List<Contract>();
    [SerializeField] List<GameObject> contractChoiceGOs = new List<GameObject>();

    [SerializeField] TextMeshProUGUI quotaText;
    [SerializeField] TextMeshProUGUI currencyText;

    [SerializeField] GameManager gameManager;

    private void Awake()
    {
        gameManager = GameManager.Instance;
    }

    public void Setup()
    {
        RandomiseContracts();
    }

    private void Update()
    {
        UpdateUIText();
    }

    void UpdateUIText()
    {
        float curQuotaMult = 0;
        float curCurrencyMult = 0;

        for (int i = 0; i < contractChoiceGOs.Count; i++)
        {
            curQuotaMult += contractChoiceGOs[i].GetComponent<ContractItem>().GetQuotaIncrease();
            curCurrencyMult += contractChoiceGOs[i].GetComponent<ContractItem>().GetBudgetIncrease();
        }

        if (curQuotaMult > 0)
        {
            quotaText.text = "Quota : " + gameManager.GetNextBaseQuota() + " + " + Mathf.Max((int)(gameManager.GetNextBaseQuota() * curQuotaMult), 1);
        }
        else
        {
            quotaText.text = "Quota : " + gameManager.GetNextBaseQuota();
        }

        if (curCurrencyMult > 0) {
            currencyText.text = "Currency : " + (int)gameManager.GetBaseCurrencyGain() + " + " + Mathf.Max((int)(gameManager.GetBaseCurrencyGain() * curCurrencyMult), 1);
        }
        else
        {
            currencyText.text = "Currency : " + (int)gameManager.GetBaseCurrencyGain();
        }
    }

    public void RandomiseContracts()
    {
        for (int i = 0; i < contractChoiceGOs.Count; i++)
        {
            Contract cur = new Contract();

            //make tiered selection eg. large neg + large pos, small neg + small pos
            cur.positive = allPositiveContractsEffects[UnityEngine.Random.Range(0, allPositiveContractsEffects.Count)];
            cur.negative = allNegativeContractsEffects[UnityEngine.Random.Range(0, allNegativeContractsEffects.Count)];

            contractChoiceGOs[i].GetComponent<ContractItem>().Setup(cur);
        }
    }

    public void CheckContractsStart()
    {
        for (int i = 0; i < contractChoiceGOs.Count; i++)
        {
            contractChoiceGOs[i].GetComponent<ContractItem>().CheckActivate();
        }
    }

    public void ActivateContract(Contract contract)
    {
        allActiveContracts.Add(contract);
        contract.positive.EnableEffect();
        contract.negative.EnableEffect();
    }
}
