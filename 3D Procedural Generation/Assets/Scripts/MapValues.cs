using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapValues : MonoBehaviour
{
    public NoiseMapParameters heightMapParameters;
    public NoiseMapParameters moistureMapParameters;

    public GradientMapValues gradientMapParameters;

    [Header("Hieght Map Values")]
    public int width;
    public int height;
    public int seed;
    public float sizeMultiplier;
    public float scale = 50f;
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;

    public float offsetX;
    public float offsetY;

    public BiomePreset[] biomes;
    public Tilemap tilemap;

    [Header("Gradient Values")]
    public float gradientThreshold;
    public float gradientIntensity;
    public float gradientIntensityPoint;


    private void OnValidate()
    {
        if (width < 1)
        {
            width = 1;
        }
        if (height < 1)
        {
            height = 1;
        }
    }
}

[System.Serializable]
public class NoiseMapParameters
{
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;

    public float offsetX;
    public float offsetY;
}

[System.Serializable]
public class GradientMapValues
{
    public float gradientThreshold;
    public float gradientIntensity;
    public float gradientIntensityPoint;
}
