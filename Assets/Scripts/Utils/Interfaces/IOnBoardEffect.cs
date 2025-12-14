using System.Collections;

public interface IOnBoardEffect
{
    public bool CheckTrigger();

    public IEnumerator Trigger();
}
