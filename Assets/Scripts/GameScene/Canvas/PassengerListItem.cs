using TMPro;
using UnityEngine;

public class PassengerListItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI amountText;


    public void Setup(string name, int available, int total)
    {
        nameText.text = name;
        amountText.text = available.ToString() + "/" + total.ToString();
    }
}
