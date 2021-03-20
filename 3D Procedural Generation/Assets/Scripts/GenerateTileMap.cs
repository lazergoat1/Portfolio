using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileMap : MonoBehaviour
{
    public BiomePreset[] biomes;
    public Tilemap tilemap;
    public MapValues mapValues;
    public Transform player;

    public static Vector2 playerPosition;

    public bool updateEverySeconds = false;
    public float updateDelay;
    float delay = 0;

    float[,] heightMap;

    void Start()
    {
        GenerateMap();
    }

    private void Update()
    {
        delay -= Time.deltaTime;

        if (updateEverySeconds)
        {
            if (delay <= 0)
            {
                tilemap.ClearAllTiles();
                GenerateMap();
                delay = updateDelay;
            }
        }
    }
  
    void GenerateMap()
    {
        heightMap = Noise.GenerateNoise(mapValues.width, mapValues.height, mapValues.seed, mapValues.offsetX, mapValues.offsetY, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

        Vector2 center = new Vector2(mapValues.width * 0.5f, mapValues.height * 0.5f);
        float[,] gradientMap = RadialGradient.GenerateGradient(mapValues.width, mapValues.height, mapValues.gradientThreshold, center, mapValues.gradientIntensityPoint, mapValues.gradientIntensity);

        for (int x = 0; x < mapValues.width; ++x)
        {
            for (int y = 0; y < mapValues.height; ++y)
            {
                tilemap.SetTile(new Vector3Int(x, y, 0), GetBiome(Mathf.Clamp01(heightMap[x, y] + gradientMap[x, y])));
            }
        }
    }

    private TileBase GetBiome(float height)
    {
        TileBase tile = biomes[0].PickRandomTile();
        float minHeight = 0f;

        foreach (BiomePreset biome in biomes)
        {
            if (height >= biome.minHeight && biome.minHeight >= minHeight)
            {
                minHeight = biome.minHeight;
                tile = biome.PickRandomTile();
            }
        }
        return tile;
    }
}
