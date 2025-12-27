using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class PassengerList : MonoBehaviour
{
    [SerializeField] Transform listParent;
    [SerializeField] GameObject listItemPrefab;
    List<PassengerListItem> items = new List<PassengerListItem>();

    [SerializeField] Transform closeTransform;
    [SerializeField] Transform openTransform;

    [SerializeField] AnimationCurve slideCurve;
    [SerializeField] float slideSpeed;
    [SerializeField] float slideAmount;
    [SerializeField] Transform contents;
    bool opening;

    private void Update()
    {
        if (opening)
        {
            slideAmount = Mathf.Clamp(slideAmount + slideSpeed * Time.deltaTime, 0f, 1f);
        }
        else
        {
            slideAmount = Mathf.Clamp(slideAmount - slideSpeed * Time.deltaTime, 0f, 1f);
        }

        contents.position = Vector3.Lerp(closeTransform.position, openTransform.position, slideCurve.Evaluate(slideAmount));
    }

    public void Setup(List<SpeciesStats> speciesTable)
    {
        for(int i = 0; i < speciesTable.Count; i++)
        {
            if (items.Count <= i) {
                PassengerListItem newItem = Instantiate(listItemPrefab, listParent).GetComponent<PassengerListItem>();
                items.Add(newItem);
            }
            Debug.Log(speciesTable[i].species.speciesName + speciesTable[i].amountRemaining + speciesTable[i].totalAmount);
            items[i].Setup(speciesTable[i].species.speciesName,speciesTable[i].amountRemaining, speciesTable[i].totalAmount);
        }
    }

    public void Toggle()
    {
        opening = !opening;
    }
}
