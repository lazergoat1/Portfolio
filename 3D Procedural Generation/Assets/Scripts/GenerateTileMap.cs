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
    private float delay = 0;

    private NoiseMapParameters heightMapValues;
    private GradientMapValues gradientMapValues;

    private float[,] heightMap;

    private System.Random random;

    void Start()
    {
        random = new System.Random(mapValues.seed);
        gradientMapValues = mapValues.gradientMapParameters;

        heightMapValues = mapValues.heightMapParameters;

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
  
    private void GenerateMap()
    {
        heightMap = Noise.GenerateNoise(mapValues.width, mapValues.height, mapValues.seed, heightMapValues.offsetX, heightMapValues.offsetY, mapValues.scale, heightMapValues.octaves, heightMapValues.persistance, heightMapValues.lacunarity);

        Vector2 center = new Vector2(mapValues.width * 0.5f, mapValues.height * 0.5f);
        float[,] gradientMap = RadialGradient.GenerateGradient(mapValues.width, mapValues.height, gradientMapValues.gradientThreshold, center, gradientMapValues.gradientIntensityPoint, gradientMapValues.gradientIntensity);

        for (int x = 0; x < mapValues.width; ++x)
        {
            for (int y = 0; y < mapValues.height; ++y)
            {
                BiomePreset biome = GetBiome(Mathf.Clamp01(heightMap[x, y] + gradientMap[x, y]));

                tilemap.SetTile(new Vector3Int(x, y, 0), biome.PickRandomTile());

                //tilemap.SetTile(new Vector3Int(x, y, 0), SpawnFoliage(biome));
            }
        }
    }

    private BiomePreset GetBiome(float height)
    {
        BiomePreset ChosenBiome = biomes[0];
        float minHeight = 0f;

        foreach (BiomePreset biome in biomes)
        {
            if (height >= biome.minHeight && biome.minHeight >= minHeight)
            {
                minHeight = biome.minHeight;
                ChosenBiome = biome;
            }
            
        }
        return ChosenBiome;
    }

    private void SpawnFoliage(BiomePreset biome, Vector3 position)
    {
        if (random.NextDouble() > 1 - (biome.foliageSpawnPercentage / 100))
        {
            Instantiate(biome.PickRandomFoliage(), position, Quaternion.identity);
        }
    }

}
