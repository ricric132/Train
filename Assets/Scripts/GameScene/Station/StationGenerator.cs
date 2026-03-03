using System.Collections.Generic;
using UnityEngine;

public class StationGenerator : MonoBehaviour
{
    [SerializeField] List<BuildingTemplateSO> stationTemplateSOs = new List<BuildingTemplateSO>();

    private void Start()
    {
        List<BuildingTemplateSO> items = GameManager.Instance.shopManager.allItems;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemType == ItemType.Station)
            {
                stationTemplateSOs.Add(items[i]);
            }
        }
    }

    public List<BuildingTemplateSO> GenerateStationChoices(int numChoices, Rarity specificRarity = Rarity.None)
    {
        List<BuildingTemplateSO> options = stationTemplateSOs;
        List<BuildingTemplateSO> picked = new List<BuildingTemplateSO>();


        for (int i = 0; i < numChoices; i++)
        {
            List<KeyValuePair<BuildingTemplateSO, float>> oddsTable = CreateOddsTable(options);

            //Debug.Log("odds table: " + oddsTable.Count);

            float rng = UnityEngine.Random.value;
            //Debug.Log("rng: " + rng);


            foreach (var item in oddsTable)
            {
                //Debug.Log("key: " + item.Key);
                //Debug.Log("odds: " + item.Value);
                if (item.Value >= rng)
                {
                    picked.Add(item.Key);
                    //Debug.Log("picked: " + picked.Count);

                    options.Remove(item.Key);
                    break;
                }
            }
        }

        return picked;
    }

    List<KeyValuePair<BuildingTemplateSO, float>> CreateOddsTable(List<BuildingTemplateSO> options)
    {
        List<KeyValuePair<BuildingTemplateSO, float>> oddsTable = new List<KeyValuePair<BuildingTemplateSO, float>>();

        float runningTotal = 0;
        for (int i = 0; i < options.Count; i++)
        {
            runningTotal += options[i].frequency;
            oddsTable.Add(new KeyValuePair<BuildingTemplateSO, float>(options[i], runningTotal));
        }

        for (int i = 0; i < oddsTable.Count; i++)
        {
            oddsTable[i] = new KeyValuePair<BuildingTemplateSO, float>(oddsTable[i].Key, oddsTable[i].Value / runningTotal);
        }

        return oddsTable;
    }
    
}
