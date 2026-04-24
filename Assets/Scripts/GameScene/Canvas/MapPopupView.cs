using System.Collections;
using UnityEngine;

public class MapPopupView : MonoBehaviour
{
    public Transform slidingTab;
    public Transform expandedTrans;
    public Transform retractedTrans;

    public float expandPercentage;
    float expandTime = 0.5f;

    bool expanding = false;

    public void Expand()
    {
        expanding = true;
        StartCoroutine(ExpandProcess());
    }

    IEnumerator ExpandProcess()
    {
        while (expandPercentage < 1)
        {
            expandPercentage = Mathf.Clamp(expandPercentage + Time.deltaTime/expandTime, 0, 1);
            slidingTab.position = CalculatePos(expandPercentage);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void Retract()
    {
        expanding = false;
        StartCoroutine(RetractProcess());
    }

    IEnumerator RetractProcess()
    {
        while (expandPercentage > 0)
        {
            expandPercentage = Mathf.Clamp(expandPercentage - Time.deltaTime/expandTime, 0, 1);
            slidingTab.position = CalculatePos(expandPercentage);
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }

    public void ToggleExpand()
    {
        Debug.Log("expanding: " + expanding);
        if (expanding)
        {
            Retract();
        }
        else
        {
            Expand();
        }
    }

    Vector3 CalculatePos(float percentage)
    {
        return Vector3.Lerp(retractedTrans.position, expandedTrans.position, percentage);
    }
}
