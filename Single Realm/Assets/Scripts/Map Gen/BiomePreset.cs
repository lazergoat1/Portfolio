using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Biome Preset", menuName = "New Biome Preset")]
public class BiomePreset : ScriptableObject
{
    public TileBase[] tiles;
    public PrefabGroup[] foliagePrefabs;
    public PrefabGroup[] enemyPrefabs;

    [Range(0, 100)]
    public float foliageSpawnPercentage;
    [Range(0, 100)]
    public float enemySpawnPercentage;

    public float minHeight;

    public TileBase PickRandomTile(System.Random RNG)
    {
        return tiles[RNG.Next(0, tiles.Length)];
    }

    public GameObject PickRandomPrefab(System.Random RNG, PrefabGroup[] prefabs)
    {
        int total = 0;

        for (int i = 0; i < prefabs.Length; i++)
        {
            total += prefabs[i].weight;
        }

        int randomNumber = RNG.Next(0, total);

        GameObject objectToReturn = null;

        for (int i = 0; i < prefabs.Length; i++)
        {
            if (randomNumber <= prefabs[i].weight)
            {
                objectToReturn = prefabs[i].PickRandomPrefab(RNG);
                return objectToReturn;
            }
            else
            {
                randomNumber -= prefabs[i].weight;
            }
        }
        Debug.LogError("did not pick an item from the loot pool");
        return objectToReturn;
    }

    [System.Serializable]
    public class PrefabGroup
    {
        public string name;
        public int weight;
        public GameObject[] Prefabs;

        public GameObject PickRandomPrefab(System.Random RNG)
        {
            return Prefabs[RNG.Next(0, Prefabs.Length)];
        }
    }
}