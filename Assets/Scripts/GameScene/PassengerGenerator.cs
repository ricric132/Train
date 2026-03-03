using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public List<SpeciesSO> fodderSpecies;
    public List<SpeciesSO> availableSpecies;

    List<SpeciesStats> speciesTable = new List<SpeciesStats>();

    [SerializeField] PassengerList passengerList;

    private void Start()
    {
        if (RunBuffData.selectedConductor == null)
        {
            UpdateSpeciesTableTotal(availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)], 5);
            UpdateSpeciesTableTotal(availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)], 5);
            UpdateSpeciesTableTotal(availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)], 5);
            UpdateSpeciesTableTotal(availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)], 5);
            UpdateSpeciesTableTotal(availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)], 5);
        }
        else
        {
            for (int i = 0; i < RunBuffData.selectedConductor.startingSpecies.Count; i++)
            {
                UpdateSpeciesTableTotal(RunBuffData.selectedConductor.startingSpecies[i], RunBuffData.selectedConductor.speciesAmt[i]);
            }
        }

    }

    public Passenger GenerateCharacterFromPool()
    {
        string name = ((Names)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Names)).Cast<Names>().Max())).ToString();

        //SpeciesSO species = availableSpecies[UnityEngine.Random.Range(0, availableSpecies.Count)];
        SpeciesSO species = null;
        int sum = 0;
        for(int i = 0; i < speciesTable.Count; i++)
        {
            sum += speciesTable[i].amountRemaining;
        }

        if (sum == 0)
        {
            species = fodderSpecies[UnityEngine.Random.Range(0, fodderSpecies.Count)];
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, sum);
            int counter = 0;
            for (int i = 0; i < speciesTable.Count; i++)
            {
                counter += speciesTable[i].amountRemaining;
                species = speciesTable[i].species;

                if (rand <= counter)
                {
                    break;
                }
            }
        }


        if (species == null)
        {
            Debug.Log("failed to generate passenger");
            return null;
        }

        PassengerInfo info = new PassengerInfo(name, species);
        Passenger cur = Instantiate(species.prefab, spawnPoint).GetComponent<Passenger>();
        cur.SetUp(info);
        cur.ManualStart();
        cur.partOfPool = true;
        UpdateSpeciesTableRemaining(species, -1);

        return cur;
    }

    public Passenger GenerateCharacter(SpeciesSO species, bool fromPool = false)
    {
        string name = ((Names)UnityEngine.Random.Range(0, (int)Enum.GetValues(typeof(Names)).Cast<Names>().Max())).ToString();

        PassengerInfo info = new PassengerInfo(name, species);

        Passenger cur = Instantiate(species.prefab, spawnPoint).GetComponent<Passenger>();
        cur.partOfPool = fromPool;
        cur.SetUp(info);

        cur.ManualStart();

        return cur;
    }

    public void UpdateSpeciesTableTotal(SpeciesSO species, int amt)
    {
        /*
        if (speciesDict.ContainsKey(species))
        {
            speciesDict[species] += amt;
            speciesSpawnable[species] += amt;
        }
        else
        {
            Debug.Log(species.speciesName + " added");
            speciesDict[species] = amt;
            speciesSpawnable[species] = amt;  
        }
        */

        SpeciesStats speciesStats = speciesTable.Find(x => x.species == species);
        if (speciesStats != null)
        {
            speciesStats.totalAmount += amt;
            UpdateSpeciesTableRemaining(species, amt);
        }
        else
        {
            speciesStats = new SpeciesStats(species, amt);
            speciesTable.Add(speciesStats);
        }

        passengerList.Setup(speciesTable);
    }

    public void UpdateSpeciesTableRemaining(SpeciesSO species, int amt) //specially generated from outside off pool should not add to this
    {
        SpeciesStats speciesStats = speciesTable.Find(x => x.species == species);
        if (speciesStats != null)
        {
            speciesStats.amountRemaining += amt;
        }
        else
        {
            Debug.Log("species not found in passenger list");
        }

        passengerList.Setup(speciesTable);
    }
}

public class SpeciesStats
{
    public SpeciesSO species;
    public int amountRemaining = 0;
    public int totalAmount = 0;
    public float rarityBias = 1; //possible thought

    public SpeciesStats(SpeciesSO _species , int _totalAmount)
    {
        species = _species;
        amountRemaining = _totalAmount;
        totalAmount = _totalAmount;
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
