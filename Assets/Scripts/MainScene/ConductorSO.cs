using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConductorSO", menuName = "ScriptableObjects/ConductorSO")]
public class ConductorSO : ScriptableObject
{
    public string name;
    public string description;
    public string flavourText;

    public List<SpeciesSO> startingSpecies;
    public List<int> speciesAmt;

    public List<BuildingTemplateSO> startingBuildings;
}
