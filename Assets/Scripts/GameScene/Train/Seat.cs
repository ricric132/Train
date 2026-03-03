using System.Collections;
using UnityEngine;

public class Seat : SnappingPoint
{
    public enum SeatOrder
    {
        Back = 0,
        Mid = 1,
        Front = 2
    }

    int boneCount;
    int chillCount;
    int warmCount; 

    public int contractDisableDuration = 0;

    [SerializeField] GameObject disbledIndicator;

    public SeatOrder seatOrder;

    public override void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (CheckActive())
        {
            disbledIndicator.SetActive(false);
        }
        else
        {
            disbledIndicator.SetActive(true);
        }
    }


    public IEnumerator NextStation() {

        if (occupiedGO != null)
        {
            yield return StartCoroutine(occupiedGO.GetComponent<Passenger>().NextStation());
        }
    }

    public Passenger GetPassenger()
    {
        Passenger passenger;
        if (occupiedGO != null && occupiedGO.TryGetComponent<Passenger>(out passenger))
        {
            return passenger;   
        }
        return null;
    }

    public bool CheckActive()
    {
        if(contractDisableDuration > 0)
        {
            return false;
        }
        //can add other conditions for other disabling effects

        return true;
    }

    public int GetBones()
    {
        return boneCount;
    }

    public void UpdateBones(int amt)
    {

        //StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerBoneGen(this, amt));
        boneCount += amt;
    }

    public int GetChill()
    {
        return chillCount;
    }

    public void UpdateChill(int amt)
    {
        //StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerBoneGen(this, amt));
        chillCount += amt;
    }

    public int GetWarm()
    {
        return warmCount;
    }

    public void UpdateWarm(int amt)
    {
        //StartCoroutine(GameManager.Instance.triggerEffectHandler.TriggerBoneGen(this, amt));
        warmCount += amt;
    }

}

