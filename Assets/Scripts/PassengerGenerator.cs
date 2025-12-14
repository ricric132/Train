using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public enum Species
{
    imp,
    fireSpirit,
    frostSpirit
}

public enum Names
{
    John,
    Jim,
    Tim
}


public class PassengerGenerator : MonoBehaviour
{
    [SerializeField] GameObject passengerPrefab;
    [SerializeField] Transform spawnPoint;

    public List<SpeciesSO> availableSpecies;

    public Passenger GenerateCharacter()
    {

        string name = ((Names)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Names)).Cast<Names>().Max())).ToString();
        SpeciesSO species = availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)];

        PassengerInfo info = new PassengerInfo(name, species);

        Passenger cur = Instantiate(species.prefab, spawnPoint).GetComponent<Passenger>();

        cur.SetUp(info);

        return cur;
    }

    public Passenger GenerateCharacter(SpeciesSO species)
    {
        string name = ((Names)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Names)).Cast<Names>().Max())).ToString();

        PassengerInfo info = new PassengerInfo(name, species);

        Passenger cur = Instantiate(species.prefab, spawnPoint).GetComponent<Passenger>();

        cur.SetUp(info);

        return cur;
    }

}

public class PassengerInfo
{
    public string name;
    public SpeciesSO species;
    public int coins;
    public int stopsRemaining;

    
    public PassengerInfo(string _name, SpeciesSO _species)
    {
        name = _name;
        species = _species;
        coins = species.baseCoins;
        stopsRemaining = species.baseStations;
    }

    public void CopyTo(PassengerInfo other)
    {
        other.name = name;
        other.species = species;
        other.coins = coins;
        other.stopsRemaining = stopsRemaining;
    }

    public String GetFullName()
    {
        return name + " the " + species.speciesName;
    }
}
