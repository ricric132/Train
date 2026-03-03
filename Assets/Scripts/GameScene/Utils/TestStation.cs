using UnityEngine;

public class TestStation : Station
{
    [SerializeField] GameObject spawnButtonPrefab;
    [SerializeField] Transform buttonParent;

    private void Start()
    {
        foreach(SpeciesSO species in passengerGenerator.availableSpecies)
        {
            GameObject button = Instantiate(spawnButtonPrefab, buttonParent);
            button.GetComponent<TestSpawnButton>().Setup(species, this);
        }
    }

}
