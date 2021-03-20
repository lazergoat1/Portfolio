using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateIsland : MonoBehaviour
{
    public BiomePreset[] biomes;
    public Tilemap tilemap;
    public MapValues mapValues;
    public Transform player;

    [Header("Chunk Values")]
    public int chunkSize;
    public int maxLoadDistance;

    int chunksVisibleInLoadDistance;

    List<Vector2> terrainChunkList = new List<Vector2>();
    Dictionary<Vector2, float> tilemapPositionValues = new Dictionary<Vector2, float>();

    float[,] heightMap;
    float[,] gradientMap;

    void Start()
    {
        chunksVisibleInLoadDistance = Mathf.RoundToInt(maxLoadDistance / chunkSize);

        LoadMapValues();
    }

    private void Update()
    {
        UpdateVisibleChunks();
    }

    private void LoadMapValues()
    {
        heightMap = Noise.GenerateNoise(mapValues.width, mapValues.height, mapValues.seed, mapValues.offsetX, mapValues.offsetY, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

        Vector2 center = new Vector2(mapValues.width * 0.5f, mapValues.height * 0.5f);
        gradientMap = RadialGradient.GenerateGradient(mapValues.width, mapValues.height, mapValues.gradientThreshold, center, mapValues.gradientIntensityPoint, mapValues.gradientIntensity);

        for (int x = 0; x < mapValues.width; ++x)
        {
            for (int y = 0; y < mapValues.height; ++y)
            {
                float xPos = x;
                float yPos = y;

                tilemapPositionValues.Add(new Vector2(xPos, yPos), Mathf.Clamp01(heightMap[x, y] + gradientMap[x, y]));
            }
        }
    }

    void UpdateVisibleChunks()
    {
        int currentChunkCoordX = Mathf.RoundToInt(player.position.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(player.position.y / chunkSize);

        for (int i = 0; i < terrainChunkList.Count; i++)
        {
            if (terrainChunkList[i].x > chunksVisibleInLoadDistance + currentChunkCoordX || terrainChunkList[i].x < -chunksVisibleInLoadDistance + currentChunkCoordX)
            {
                UnloadChunk(terrainChunkList[i]);
            }
            else if (terrainChunkList[i].y > chunksVisibleInLoadDistance + currentChunkCoordY || terrainChunkList[i].y < -chunksVisibleInLoadDistance + currentChunkCoordY)
            {
                UnloadChunk(terrainChunkList[i]);
            }
        }

        for (int x = -chunksVisibleInLoadDistance; x < chunksVisibleInLoadDistance; x++)
        {
            for (int y = -chunksVisibleInLoadDistance; y < chunksVisibleInLoadDistance; y++)
            {
                Vector2 chunkCoord = new Vector2(currentChunkCoordX + x, currentChunkCoordY + y);

                if (!terrainChunkList.Contains(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                    terrainChunkList.Add(chunkCoord);
                }
            }
        }
    }

    void LoadChunk(Vector2 chunkCoord)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();
        List<TileBase> tiles = new List<TileBase>();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                int xPos = x + Mathf.RoundToInt(chunkCoord.x * chunkSize);
                int yPos = y + Mathf.RoundToInt(chunkCoord.y * chunkSize);

                Vector3Int newTilePosition = new Vector3Int(xPos, yPos, 0);

                tilePositions.Add(newTilePosition);
                if(tilemapPositionValues.ContainsKey(new Vector2(xPos, yPos)))
                {
                    tiles.Add(GetBiome(tilemapPositionValues[new Vector2(xPos, yPos)]));
                }
                else
                {
                    tiles.Add(GetBiome(0f));
                }
            }
        }
        tilemap.SetTiles(tilePositions.ToArray(), tiles.ToArray());
    }

    void UnloadChunk(Vector2 chunkCoord)
    {
        List<Vector3Int> tilePositions = new List<Vector3Int>();
        List<TileBase> tiles = new List<TileBase>();

        for (int x = 0; x < chunkSize; ++x)
        {
            for (int y = 0; y < chunkSize; ++y)
            {
                int xPos = x + Mathf.RoundToInt(chunkCoord.x * chunkSize);
                int yPos = y + Mathf.RoundToInt(chunkCoord.y * chunkSize);

                Vector3Int newTilePosition = new Vector3Int(xPos, yPos, 0);

                tilePositions.Add(newTilePosition);
                tiles.Add(null);
            }
        }
        tilemap.SetTiles(tilePositions.ToArray(), tiles.ToArray());
        terrainChunkList.Remove(chunkCoord);
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
