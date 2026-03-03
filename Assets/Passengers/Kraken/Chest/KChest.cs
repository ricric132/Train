using UnityEngine;
using System.Collections;
using TMPro;

public class KChest : StationaryItem, IOnMoneyEarntEffect
{
    float gold = 3;
    [SerializeField] TextMeshProUGUI goldText; 

    public override void Start()
    {
        base.Start();
        triggerEffectHandler.AddEffect(gameObject);
        goldText.text = ((int)gold).ToString();
    }

    public IEnumerator OnMoneyEarnt(int amt)
    {
        Debug.Log("earnt " + amt);
        gold += amt/10.0f;
        goldText.text = ((int)gold).ToString();
        yield return null;
    }

    public override void UseItem(Passenger p)
    {
        playerManager.AddPositive((int)gold);
        base.UseItem(p);
    }

    public override bool IsUsableTarget(SnappingPoint snap)
    {
        if(snap is Seat s && s.GetPassenger() && s.GetPassenger() is not StationaryItem)
        {
            return true;
        }

        return false;
    }

}
