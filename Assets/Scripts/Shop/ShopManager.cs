using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    //public List<ItemTemplateSO> allItems;
    public List<BuildingTemplateSO> allItems;

    public List<ShopSlot> shopSlots;
    public Camera mapCam;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BuildingTemplateSO GetRandomItemTemplate()
    {
        //add diff odds and stuff later;
        return allItems[UnityEngine.Random.Range(0, allItems.Count)];
    }

    public ShopItem GetRandomItem()
    {
        Debug.Log("got random Items");
        BuildingTemplateSO template = GetRandomItemTemplate();
        GameObject go = Instantiate(template.itemPrefab);
        //do some manipulations to it eg change price
        go.GetComponent<ShopItem>().templateSO = template;
        go.GetComponent<ShopItem>().cam = mapCam;   

        return go.GetComponent<ShopItem>();
    }

    public void RandomizeShop()
    {
        for (int i = 0; i < shopSlots.Count; i++)
        {
            shopSlots[i].RandomizeItem();
        }
    }

    public void SetUp()
    {
        RandomizeShop();
    }

    public void LeaveShop()
    {
        StartCoroutine(gameManager.StartNextDay());
    }
}
