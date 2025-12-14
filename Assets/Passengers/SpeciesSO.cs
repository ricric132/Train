using UnityEngine;

[CreateAssetMenu(fileName = "SpeciesSO", menuName = "ScriptableObjects/SpeciesSO")]
public class SpeciesSO : ScriptableObject
{
    public string speciesName;
    [TextArea] public string description;
    public int baseCoins;
    public int baseStations;

    public GameObject prefab;
}
