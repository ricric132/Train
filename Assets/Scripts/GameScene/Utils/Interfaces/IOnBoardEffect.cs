using System.Collections;

public interface IOnBoardEffect
{
    public bool OnBoardCheckTrigger(Passenger p);

    public IEnumerator OnBoardTrigger(Passenger p);
}
