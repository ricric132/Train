using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

public class StationaryItem : Passenger
{
    public override bool UpdateStationsRemaining(int amount)
    {
        return false;
    }

    public override void UpdateCoins(int amount)
    {
        
    }


    public override void HandleSnap(SnappingPoint snapPoint)
    {
        if (snapPoint.tag == "DepartPoint")
        {
            if (seat != null)
            {
                //Debug.Log("departed");

                //seat.occupiedGO = null;
                prevParentSnap.occupiedGO = null;
                transform.parent = snapPoint.transform;
                transform.localPosition = Vector3.zero;
                UseItem(null);
            }
            else
            {
                ReturnToStart();
            }
        }
        else if (!canOverlap && snapPoint.occupiedGO != null && snapPoint.occupiedGO != gameObject)
        {
            if(snapPoint is Seat targetSeat && targetSeat.GetPassenger() && targetSeat.GetPassenger() is not StationaryItem)
            {
                UseItem(targetSeat.GetPassenger());
            }
            ReturnToStart();
        }
        else if (seat != null)
        {
            ReturnToStart();
        }
        else
        {
            Seat snapPointSeat = null;
            snapPoint.gameObject.TryGetComponent<Seat>(out snapPointSeat);

            if (snapPointSeat != null)
            {
                if (snapPointSeat.CheckActive())
                {
                    SnapTo(snapPoint);
                    OnSeated(snapPointSeat);
                }
                else
                {
                    ReturnToStart();
                }
            }
            else
            {
                SnapTo(snapPoint);
                seat = null;
            }
        }
    }

    public virtual void UseItem(Passenger p)
    {
        Remove();
    }

    public virtual bool IsUsableTarget(SnappingPoint snap)
    {
        return false;
    }
}
