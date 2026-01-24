using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "BuildingTemplateSO", menuName = "ScriptableObjects/BuildingTemplateSO")]
public class BuildingTemplateSO : ScriptableObject
{
    public ItemType itemType;
    public Rarity rarity;
    public int baseCost;

    public string buildingName;
    [TextArea] public string description;

    public GameObject prefab;
    public GameObject mapPrefab;
    public GameObject itemPrefab;

    public List<Vector2Int> occupiedSpaces = new List<Vector2Int>() {Vector2Int.zero};

    public int frequency;

    public string GetTag()
    {
        if (mapPrefab)
        {
            return mapPrefab.tag;
        }

        return null;
    }
}

public enum ItemType
{
    Station,
    Building,
}

public enum Rarity
{
    Common,
    Rare,
}
