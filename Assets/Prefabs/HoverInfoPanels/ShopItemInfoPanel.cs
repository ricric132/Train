using TMPro;
using UnityEngine;

public class ShopItemInfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI costText;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI keywords;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetUp(BuildingTemplateSO item, int priceOverride = -1)
    {
        
        nameText.text = item.buildingName;
        costText.text = item.baseCost.ToString();
        if(priceOverride > 0)
        {
            costText.text = priceOverride.ToString();
        }
       
        description.text = item.description;
        keywords.text = item.itemType.ToString();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
}
