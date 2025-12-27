using TMPro;
using UnityEngine;

public class EndOfDayPopup : MonoBehaviour
{
    public TextMeshProUGUI quotaText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI successText;
    public GameManager gameManager;

    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void Setup(int quota, int coinAmt) //want to add like an overall passenger log
    {
        quotaText.text = "Quota amount : " + quota;
        coinText.text = "Money Earnt : " + coinAmt;

        if(coinAmt >= quota)
        {
            successText.text = "Quota Met";
        }
        else
        {
            successText.text = "Quota NOT Met";
        }

    }
}
