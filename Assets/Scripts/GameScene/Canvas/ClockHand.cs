using UnityEngine;

public class ClockHand : MonoBehaviour
{
    [SerializeField] Transform hand;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float zRot = Mathf.Lerp(90, -90, GameManager.Instance.dayClock.GetPercentage(GameManager.Instance.clockTimer)) ;
        hand.eulerAngles = new Vector3(0, 0, zRot);   
    }
}
