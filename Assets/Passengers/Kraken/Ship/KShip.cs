using System.Collections;
using TMPro;
using UnityEngine;

public class KShip : StationaryItem, IOffBoardEffect
{
    int crew = 2;
    int offboarded = 0;
    [SerializeField] TextMeshProUGUI counterText;

    public override void Start()
    {
        base.Start();
        triggerEffectHandler.AddEffect(gameObject);
        counterText.text = crew.ToString();
    }

    public IEnumerator OffBoardTrigger(Passenger p)
    {
        offboarded++;
        crew = 2 + offboarded / 2;
        counterText.text = crew.ToString();
        yield return null;
    }

    public override void UseItem(Passenger p)
    {
        for (int i = 0; i < crew; i++) 
        {
            Debug.Log("spawning");
            path.GetCurStation().AddPassengerFromPool();
        }

        base.UseItem(p);
    }

    public override bool IsUsableTarget(SnappingPoint snap)
    {
        if (snap is Seat s && s.GetPassenger() && s.GetPassenger() is not StationaryItem)
        {
            return true;
        }

        return false;
    }
}
