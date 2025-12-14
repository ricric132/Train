using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BuildableItem : ShopItem
{
    public GameObject mapVisualValid;
    public GameObject mapVisualInvalid;
    public List<BuildRule> buildRules;
    public BuildingTemplateSO buildingTemplate;

    public override void Start()
    {
        base.Start();
        BuildRule[] rules = GetComponents<BuildRule>();
        for (int i = 0; i < rules.Length; i++)
        {
            buildRules.Add(rules[i]);
        }
    }

    public override void Update()
    {
        base.Update();

        if (OnMap())
        {
            if (IsValidBuildSpot())
            {
                mapVisualValid.SetActive(true);
                mapVisualInvalid.SetActive(false);
            }
            else
            {
                mapVisualValid.SetActive(false);
                mapVisualInvalid.SetActive(true);
            }
        }
    }

    
    bool IsValidBuildSpot()
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(transform.position);
        Vector2Int coords = map.WorldPosToGridCoord(worldPos.x, worldPos.y);

        if(coords.x < 0 || coords.x >= map.grid.GetLength(0))
        {
            return false;
        }
        if (coords.y < 0 || coords.y >= map.grid.GetLength(1))
        {
            return false;
        }

        for (int i = 0; i < buildRules.Count; i++)
        {
            //Debug.Log(coords.x + ", " + coords.y);
            if(buildRules[i] && buildRules[i].Check(map.grid, new Vector2Int(coords.x, coords.y)) == false)
            {
                return false;
            }
        }

        return true;
    }

    public override void OnPointerUp()
    {
        base.OnPointerUp();

        if (OnMap() && IsValidBuildSpot())
        {
            Vector2 worldPos = cam.ScreenToWorldPoint(transform.position);
            Vector2Int coords = map.WorldPosToGridCoord(worldPos.x, worldPos.y);
            
            Build(coords);
        }
        else
        {
            transform.localPosition = Vector3.zero;
            mapVisual.SetActive(false);
            itemVisual.SetActive(true);
        }

    }

    void Build(Vector2Int buildSpot)
    {
        map.Build(buildSpot, Rotation.left, templateSO);
        Destroy(gameObject);
    }

}
