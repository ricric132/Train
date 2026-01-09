using Unity.VisualScripting;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;


public class Imp : Passenger
{
    [SerializeField] SpeciesSO impSO;
    public int drunkness = 0;

    public override void DestinationReachedEffect()
    {
        Debug.Log("attempt spawn");
        Passenger spawn =  path.GetCurStation().AddPassenger(impSO);
        if (spawn != null)
        {
            Imp imp = (Imp)spawn;
            imp.drunkness = drunkness+1;

            imp.info.coins = (int)(info.coins * 1.2);
            
        }
    }

    public override void DoSeatedEffect(Seat _seat)
    {
        base.DoSeatedEffect(_seat);
        
        if(Random.value < drunkness * 0.1f)
        {
            UpdateStationsRemaining(10);
        }
    }

    public override void OnMouseEnter()
    {
        base.OnMouseEnter();
        if(drunkness > 0)
        {
            activeInfoPanel.GetComponent<InfoPanel>().SetUp(info, "Drunk" + "x" + drunkness.ToString() + info.name + " the " + info.species.speciesName, "OnBoard : has a " + (drunkness * 10).ToString()
            + "% chance to fall sleep\n" + info.species.description);
        }

    }

}
