using TMPro;
using UnityEngine;

public class InfoPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI stopsRemainingText;
    [SerializeField] TextMeshProUGUI description; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rect rect = GetComponent<RectTransform>().rect;
        Debug.Log(rect.x + " " + rect.y);
        transform.position = Input.mousePosition;
    }

    public void SetUp(PassengerInfo info, string oName = "", string oDesc = "")
    {
        nameText.text = info.GetFullName();
        coinText.text = "Fare: " + info.coins.ToString();
        stopsRemainingText.text = "Stops Remaining: " + info.stopsRemaining.ToString();
        description.text = info.species.description;

        if(oName != "")
        {
            nameText.text = oName;
        }

        if(oDesc != "")
        {
            description.text = oDesc;
        }
    }

    


}
