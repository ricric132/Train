using System.Collections.Generic;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{ 
    [SerializeField] GameManager gameManager;
    PlayerManager playerManager;

    //public List<ItemTemplateSO> allItems;
    public List<BuildingTemplateSO> allItems;

    public GameObject slotsParents;
    public List<ShopSlot> shopSlots;

    public GameObject ShopCanvas;
    public Camera mapCam;

    [SerializeField] TextMeshProUGUI moneyTextInShop;
    [SerializeField] TextMeshProUGUI moneyTextOutOfShop;

    [SerializeField] TextMeshProUGUI invalidText;
    float invalidPopupDur = 1;
    float timeSinceLastInvalid;

    int rerollCost = 2;

    bool isActive = false;
    public GameObject bottemBar;
    public Vector3 bottemBarDefaultOffset;
    public GameObject topBar;
    public Vector3 topBarDefaultOffset;

    float transitionTime = 0;
    float totalTransitionTime = 1;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        playerManager = gameManager.playerManager;
    }

    // Update is called once per frame
    void Update()
    {
        moneyTextInShop.text = "money : " + playerManager.GetMoney();
        moneyTextOutOfShop.text = "money : " + playerManager.GetMoney();

        timeSinceLastInvalid += Time.deltaTime;
        if (timeSinceLastInvalid > invalidPopupDur)
        {
            invalidText.text = "";
        }

    }

    public void toggle(bool newActive)
    {
        Debug.Log("new active: " + newActive);
        if(newActive != isActive)
        {
            isActive = newActive;

            if (newActive)
            {
                StartCoroutine(OpenAnim());
            }
            else
            {
                StartCoroutine(CloseAnim());
            }
        }
    }

    //make better anims later
    IEnumerator OpenAnim()
    {

        while (transitionTime < totalTransitionTime)
        {
            topBar.transform.localPosition = Vector3.Lerp(topBarDefaultOffset, Vector3.zero, (Mathf.Min(1, transitionTime / totalTransitionTime)));
            bottemBar.transform.localPosition = Vector3.Lerp(bottemBarDefaultOffset, Vector3.zero, (Mathf.Min(1, transitionTime / totalTransitionTime)));

            transitionTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transitionTime = totalTransitionTime;

        topBar.transform.localPosition = Vector3.zero;
        bottemBar.transform.localPosition = Vector3.zero;
    }

    IEnumerator CloseAnim()
    {
        while (transitionTime > 0)
        {
            topBar.transform.localPosition = Vector3.Lerp(topBarDefaultOffset, Vector3.zero, (Mathf.Min(1, transitionTime / totalTransitionTime)));
            bottemBar.transform.localPosition = Vector3.Lerp(bottemBarDefaultOffset, Vector3.zero, (Mathf.Min(1, transitionTime / totalTransitionTime)));

            transitionTime -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transitionTime = 0;

        topBar.transform.localPosition = topBarDefaultOffset;
        bottemBar.transform.localPosition = bottemBarDefaultOffset;
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

    public void BuyShopReroll()
    {
        if (playerManager.GetMoney() >= rerollCost)
        {
            playerManager.UpdateMoney(-rerollCost);
            RandomizeShop();
        }
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
        shopSlots = slotsParents.GetComponentsInChildren<ShopSlot>().ToList();
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


/* Canvas Implementation
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

    int rerollCost = 2; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        playerManager = gameManager.playerManager;
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

    public void BuyShopReroll()
    {
        if(playerManager.GetMoney() >= rerollCost)
        {
            playerManager.UpdateMoney(-rerollCost);
            RandomizeShop();
        }
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
*/
