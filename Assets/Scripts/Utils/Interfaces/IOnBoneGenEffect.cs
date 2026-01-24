using System.Collections;
using UnityEngine;

public interface IOnBoneGenEffect
{
    public IEnumerator OnBoneGen(Seat seat, int boneAmt);
}
