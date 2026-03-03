using TMPro;
using UnityEngine;

public class ConductorPanel : MonoBehaviour
{
    public ConductorSO conductorSO;

    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] TextMeshProUGUI flavourText;

    public void SetupPanel(ConductorSO conductor)
    {
        conductorSO = conductor;

        nameText.text = conductor.name;
        descText.text = conductor.description;
        flavourText.text = conductor.flavourText;
    }
}
