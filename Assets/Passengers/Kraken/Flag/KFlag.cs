using UnityEngine;
using System.Collections;
using TMPro;

public class KFlag : StationaryItem
{ 
    int inspiration = 2;
    [SerializeField] TextMeshProUGUI counterText;
    public override void Start()
    {
        base.Start();
        triggerEffectHandler.AddEffect(gameObject);
        counterText.text = inspiration.ToString();
    }

    public override IEnumerator NextStationAction()
    {
        inspiration += 1;
        counterText.text = inspiration.ToString();
        yield return null;
    }

    public override void UseItem(Passenger p)
    {
        p.UpdateStationsRemaining(-inspiration/2);
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
