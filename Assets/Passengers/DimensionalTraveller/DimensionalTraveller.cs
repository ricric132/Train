using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DimensionalTraveller : Passenger
{
    HashSet<BuildingTemplateSO> visited = new HashSet<BuildingTemplateSO>();

    public override IEnumerator NextStationAction()
    {
        StartCoroutine(base.NextStationAction());

        if (!visited.Contains(path.GetCurStation().stationTemplate))
        {
            visited.Add(path.GetCurStation().stationTemplate);
            UpdateCoins(visited.Count);
        }

        yield return null;

    }

}
