using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PlayerManager playerManager;

    //public List<ItemTemplateSO> allItems;
    public List<BuildingTemplateSO> allItems;

    public List<ShopSlot> shopSlots;

    public GameObject ShopCanvas;
    public Camera mapCam;

    [SerializeField] TextMeshProUGUI moneyTextInShop;
    [SerializeField] TextMeshProUGUI moneyTextOutOfShop;

    [SerializeField] TextMeshProUGUI invalidText;
    float invalidPopupDur = 1;
    float timeSinceLastInvalid;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        playerManager = FindFirstObjectByType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyTextInShop.text = "money : " + playerManager.GetMoney();
        moneyTextOutOfShop.text = "money : " + playerManager.GetMoney();

        timeSinceLastInvalid += Time.deltaTime;
        if (timeSinceLastInvalid > invalidPopupDur) {
            invalidText.text = "";
        }

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
        Debug.Log("shop setup started");
        shopSlots.Clear();
        shopSlots = ShopCanvas.GetComponentsInChildren<ShopSlot>().ToList();
        for (int i = 0; i < shopSlots.Count; i++)
        {
            shopSlots[i].shopManager = this;
            shopSlots[i].infoPanel.gameObject.SetActive(false);
        }
        RandomizeShop();
    }

    public int GetCost(BuildingTemplateSO item)
    {
        //Do all Cost Modifiers
        return item.baseCost;
    }

    public bool CheckAffordable(ShopItem item)
    {
        return playerManager.GetMoney() >= GetCost(item.templateSO); //add cost change and stuff later
    }

    public void Buy(ShopItem item)
    {
        playerManager.UpdateMoney(-item.templateSO.baseCost);
    }


    public void LeaveShop()
    {
        gameManager.ToggleShop();
        //StartCoroutine(gameManager.StartNextDay());
    }

    public void InvalidActionPopup(string str)
    {
        invalidText.text = str;
        timeSinceLastInvalid = 0;
    }
}
