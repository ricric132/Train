using System.Collections;
using UnityEngine;

public class Seat : SnappingPoint
{
    
    public override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
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

}
