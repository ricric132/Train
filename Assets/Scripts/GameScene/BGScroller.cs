using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 5;
    [SerializeField] List<Transform> trackTiles;
    float curOffset = 0;
    [SerializeField] float tileSpacing;
    [SerializeField] float bottemCullingPoint;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bottemCullingPoint = tileSpacing * 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.trainManager.stopped)
        {
            curOffset += scrollSpeed * Time.deltaTime;
            for (int i = 0; i < trackTiles.Count; i++)
            {
                trackTiles[i].localPosition = new Vector3(0, -((curOffset + (i * tileSpacing)) % bottemCullingPoint), 0);
            }
        }
    }
}
