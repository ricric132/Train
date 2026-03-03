using UnityEngine;

public class ClockTime
{
    float start = 6 * 60f;
    float end = 24 * 60f;

    public string GetString(float time)
    {
        float curTime = start + time;
        if(((int)curTime/(12*60)) % 2 == 0){
            return (((int)curTime%(12*60))/60).ToString("D2") + ":" + ((int)curTime%60).ToString("D2") + "am";
        }

        return (((int)curTime % (12 * 60)) / 60).ToString("D2") + ":" + ((int)curTime % 60).ToString("D2") + "pm";
    }

    public bool CheckDayEnd(float time)
    {
        return time > end - start;
    }

    public float GetPercentage(float time)
    {
        return time / (end - start);
    }

    public ClockTime(float start, float end)
    {
        this.start = start;
        this.end = end;
    }
}
