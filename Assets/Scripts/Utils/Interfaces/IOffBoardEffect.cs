using UnityEngine;
using System.Collections;

public interface IOffBoardEffect
{
    public IEnumerator OffBoardTrigger(Passenger p);
}
