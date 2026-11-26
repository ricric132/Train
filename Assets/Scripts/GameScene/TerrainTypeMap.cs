using UnityEngine;
using UnityEngine.Rendering;

public class TerrainTypeMap : MonoBehaviour
{
    [SerializeField] static SerializedDictionary<TerrainType, GameObject> mapping;

    public static GameObject GetTerrain(TerrainType type)
    {
        return mapping[type];
    }
}

public enum TerrainType
{
    none,
    grass,
    mountain,
    water
}

