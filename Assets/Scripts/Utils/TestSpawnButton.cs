using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestSpawnButton : MonoBehaviour
{
    public void Setup(SpeciesSO species, TestStation testStation)
    {
        GetComponent<Button>().onClick.AddListener(() => testStation.AddPassenger(species));
        GetComponentInChildren<TextMeshProUGUI>().text = species.speciesName;
    }
}
