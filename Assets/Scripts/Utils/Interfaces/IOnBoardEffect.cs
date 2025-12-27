using System.Collections;

public interface IOnBoardEffect
{
    public bool CheckTrigger(Passenger p);

    public IEnumerator Trigger(Passenger p);
}
