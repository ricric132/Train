using UnityEngine;


public class ContractEffect : ScriptableObject
{
    [TextArea]public string description;

    public ContractEffect()
    {
        
    }

    public virtual void EnableEffect(GameManager gm)
    {

    }

    public virtual void RevertEffect(GameManager gm)
    {

    }

    public virtual string GetDescription() 
    {
        return "";
    }
}
