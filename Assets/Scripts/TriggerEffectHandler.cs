using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEffectHandler : MonoBehaviour
{
   
    List<IOnBoardEffect> onboardEffects = new List<IOnBoardEffect>();
    List<IOffBoardEffect> offboardEffects = new List<IOffBoardEffect>();
    List<IOnEnterStationEffect> enterStationEffects = new List<IOnEnterStationEffect>();
    List<IOnBoneGenEffect> boneGenEffects = new List<IOnBoneGenEffect>();
    List<IOnMoneyEarntEffect> onMoneyEarntEffects = new List<IOnMoneyEarntEffect>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddEffect(GameObject obj)
    {
        //Debug.Log("adding");
        var tempMonoArray = obj.GetComponents<MonoBehaviour>();

        foreach (var monoBehaviour in tempMonoArray)
        {
            if (monoBehaviour is IOnBoardEffect boardEffect)
            {
                //Debug.Log("successful addd" + monoBehaviour.name);

                onboardEffects.Add(boardEffect);
            }

            if (monoBehaviour is IOffBoardEffect offBoardEffect)
            {
                offboardEffects.Add(offBoardEffect);
            }

            if (monoBehaviour is IOnEnterStationEffect stationEnterEffect)
            {
                enterStationEffects.Add(stationEnterEffect);
            }

            if (monoBehaviour is IOnBoneGenEffect boneGenEffect)
            { 
                boneGenEffects.Add(boneGenEffect);
            }

            if (monoBehaviour is IOnMoneyEarntEffect onMoneyEarnt)
            {
                onMoneyEarntEffects.Add(onMoneyEarnt);
            }
        }
    }

    public void RemoveEffect(GameObject obj)
    {
        var tempMonoArray = obj.GetComponents<MonoBehaviour>();

        foreach (var monoBehaviour in tempMonoArray)
        {
            if (monoBehaviour is IOnBoardEffect boardEffect)
            {
                onboardEffects.Remove(boardEffect);
            }

            if (monoBehaviour is IOffBoardEffect offBoardEffect)
            {
                offboardEffects.Remove(offBoardEffect);
            }

            if (monoBehaviour is IOnEnterStationEffect stationEnterEffect)
            {
                enterStationEffects.Remove(stationEnterEffect);
            }

            if (monoBehaviour is IOnBoneGenEffect boneGenEffect)
            {
                boneGenEffects.Remove(boneGenEffect);
            }

            if (monoBehaviour is IOnMoneyEarntEffect onMoneyEarnt)
            {
                onMoneyEarntEffects.Remove(onMoneyEarnt);
            }
        }
    }

    public IEnumerator TriggerOnBoard(Passenger p)
    {
        //Debug.Log("boardinggggg");
        for(int i = 0; i < onboardEffects.Count; i++)
        {
            if (onboardEffects[i].OnBoardCheckTrigger(p))
            {
                StartCoroutine(onboardEffects[i].OnBoardTrigger(p));
            }
        }

        yield return null;
    }

    public IEnumerator TriggerOffBoard(Passenger p)
    {
        Debug.Log("off boarding");
        for (int i = 0; i < offboardEffects.Count; i++)
        {
            StartCoroutine(offboardEffects[i].OffBoardTrigger(p));
        }

        yield return null;
    }

    public IEnumerator TriggerOnEnterStation(Station station)
    {
        for (int i = 0; i < enterStationEffects.Count; i++)
        {
            StartCoroutine(enterStationEffects[i].OnEnterStationTrigger());
        }

        yield return null;
    }

    public IEnumerator TriggerBoneGen(Seat seat, int boneAmt)
    {
        for (int i = 0; i < boneGenEffects.Count; i++)
        {
            StartCoroutine(boneGenEffects[i].OnBoneGen(seat, boneAmt));
        }

        yield return null;
    }

    public IEnumerator TriggerOnMoneyEarnt(int amt)
    {
        for (int i = 0; i < onMoneyEarntEffects.Count; i++)
        {
            StartCoroutine(onMoneyEarntEffects[i].OnMoneyEarnt(amt));
        }

        yield return null;
    }
}
