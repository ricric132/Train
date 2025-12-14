using UnityEngine;

public class ShopSlot : MonoBehaviour
{
    ShopManager shopManager;
    ShopItem curItem;

    void Awake()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
        shopManager.shopSlots.Add(this);    
    }

    void Update()
    {
        
    }

    public void RandomizeItem()
    {
        if(curItem && curItem.gameObject)
        {
            Destroy(curItem.gameObject);
            curItem = null;
        }

        if (shopManager == null)
        {
            Debug.Log("no shop");
        }

        ShopItem item = shopManager.GetRandomItem();
        item.slot = this;
        curItem = item;
        item.gameObject.transform.parent = transform;
        item.gameObject.transform.localPosition = Vector3.zero;
        item.gameObject.transform.localScale = Vector3.one;

    }


}
