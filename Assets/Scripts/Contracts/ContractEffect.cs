using UnityEngine;


public class ContractEffect : ScriptableObject
{
    [TextArea]public string description;

    public ContractEffect()
    {
        
    }

    public virtual void EnableEffect()
    {

    }

    public virtual void RevertEffect()
    {

    }

    public virtual string GetDescription() 
    {
        return "";
    }
}
