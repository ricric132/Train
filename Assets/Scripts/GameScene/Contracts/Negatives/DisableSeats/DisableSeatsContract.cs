using UnityEngine;

[CreateAssetMenu(fileName = "DisableSeats", menuName = "ScriptableObjects/ContractEffects/DisableSeats")]

public class DisableSeatsContract : ContractEffect
{
    public int seatDisabled;
    public DisableSeatsContract() : base()
    {
        description = "Disable " + (seatDisabled) + " seats for next day";
    }

    public override void EnableEffect()
    {
        GameManager.Instance.trainManager.ContractDisableSeats(seatDisabled, 1);
    }

    public override void RevertEffect()
    {

    }
}
