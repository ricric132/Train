using UnityEngine;

public class PassengerAnimator : MonoBehaviour
{
    public Animator offsetAnimator;

    public virtual void NewStationEffectAnim()
    {
        DoEffectAnim();
    }

    public virtual void OnboardAnim()
    {
        DoEffectAnim();
    }

    public virtual void DoEffectAnim()
    {
        offsetAnimator.SetTrigger("Wiggle");
    }
}
