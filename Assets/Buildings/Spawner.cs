using UnityEngine;
using System.Collections.Generic;

public class Spawner : Building
{
    [SerializeField] List<SpeciesSO> species;
    [SerializeField] List<int> amounts;

    public void OnEnable()
    {
        AddPassengers();
    }

    public void OnDisable()
    {
        RemovePassengers();
    }

    public void AddPassengers()
    {
        for (int i = 0; i < species.Count; i++) {
            GameManager.Instance.passengerGenerator.UpdateSpeciesTableTotal(species[i], amounts[i]);
        }
    }
    public void RemovePassengers()
    {
        for (int i = 0; i < species.Count; i++)
        {
            GameManager.Instance.passengerGenerator.UpdateSpeciesTableTotal(species[i], -amounts[i]);
        }
    }
}
