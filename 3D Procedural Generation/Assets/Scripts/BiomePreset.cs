using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    public TileBase[] tiles;
    public GameObject[] foliagePrefabs;
    public NoiseMapParameters foliageMapParameters;
    public float minHeight;
    public float minMoisture;

    [Range(0, 100)]
    public float foliageSpawnPercentage;

    public TileBase PickRandomTile()
    {
        return tiles[Random.Range(0, tiles.Length)];
    }
    public GameObject PickRandomFoliage()
    {
        return foliagePrefabs[Random.Range(0, foliagePrefabs.Length)];
    }
}
