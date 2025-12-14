using System.Collections.Generic;
using UnityEngine;

public class SpawnEffectPopup : MonoBehaviour
{
    public enum popupType
    {
        coin = 0,
        time = 1
    }
    public List<GameObject> popups;

    float popupSpacing;
    float lifeSpan = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnPopup(Transform spawnPoint, int addedNumber, popupType type)
    {
        Debug.Log("Popup");
        GameObject effect = Instantiate(popups[(int)type], spawnPoint);
        effect.GetComponent<EffectPopup>().SetUp(addedNumber, lifeSpan);
    }
}
