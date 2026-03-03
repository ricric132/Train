using System.Collections;
using UnityEngine;

public interface IOnMoneyEarntEffect
{
    public IEnumerator OnMoneyEarnt(int amt);
}
