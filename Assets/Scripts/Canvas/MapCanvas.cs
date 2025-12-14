using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class MapCanvas : MonoBehaviour
{
    public GameObject queueParent;
    List<GameObject> queue = new List<GameObject>();
    public GameObject stationBlockPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateQueueVisual(List<Station> path)
    {
        GameObject station = Instantiate(stationBlockPrefab, queueParent.transform);
        queue.Add(station);
    }

    
}
