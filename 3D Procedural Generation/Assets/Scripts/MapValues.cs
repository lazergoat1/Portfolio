using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapValues : MonoBehaviour
{
    public BiomePreset[] biomes;
    public Tilemap tilemap;

    public int width;
    public int height;
    public float gradientThreshold;
    public float gradientIntensity;
    public float gradientIntensityPoint;

    public int seed;
    public float sizeMultiplier;
    public float scale = 50f;
    public int octaves = 4;
    [Range(0, 1)]
    public float persistance = 0.5f;
    public float lacunarity = 2f;

    public float offsetX;
    public float offsetY;

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
