using TMPro;
using UnityEngine;

public class CarBuffIndicator : MonoBehaviour
{
    [SerializeField] GameObject boneObject;
    [SerializeField] TextMeshProUGUI boneText;

    [SerializeField] GameObject chillObject;
    [SerializeField] TextMeshProUGUI chillText;

    [SerializeField] GameObject warmObject;
    [SerializeField] TextMeshProUGUI warmText;

    public void UpdateText(int boneAmt, int chillAmt, int warmAmt)
    {
        if(boneAmt > 0)
        {
            boneObject.SetActive(true);
            boneText.text = boneAmt.ToString();
        }
        else
        {
            boneObject.SetActive(false);
        }

        if(chillAmt > 0)
        {
            chillObject.SetActive(true);
            chillText.text = chillAmt.ToString();
        }
        else
        {
            chillObject.SetActive(false);
        }

        if (warmAmt > 0)
        {
            warmObject.SetActive(true);
            warmText.text = warmAmt.ToString();
        }
        else
        {
            warmObject.SetActive(false);    
        }
    }
}
