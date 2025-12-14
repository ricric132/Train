using UnityEngine;

[CreateAssetMenu(fileName = "ItemTemplateSO", menuName = "ScriptableObjects/ItemTemplateSO")]
public class ItemTemplateSO : ScriptableObject
{
    public ItemType itemType;
    public Rarity rarity;
    public int baseCost;

    public ScriptableObject itemSO;
    public GameObject go;
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