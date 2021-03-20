using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileMapInChunks : MonoBehaviour
{
    public Transform player;
    public MapValues mapValues;
    public Tilemap tilemap;
    public BiomePreset[] biomes;

    [Header("Dimensions")]
    public int chunkSize;
    public int maxLoadDistance;

    float[,] heightMap;
    int chunksVisibleInLoadDistance;

    List<Vector2> terrainChunkList = new List<Vector2>();

    void Start()
    {
        chunksVisibleInLoadDistance = Mathf.RoundToInt(maxLoadDistance / chunkSize);
    }

    private void Update()
    {
        UpdateVisibleChunks();
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
 
                if(!terrainChunkList.Contains(chunkCoord))
                {
                    LoadChunk(chunkCoord);
                    terrainChunkList.Add(chunkCoord);
                }
            }
        }
    }

    void LoadChunk(Vector2 chunkCoord)
    {
        heightMap = Noise.GenerateTileMapNoise(chunkSize, chunkSize, mapValues.seed, chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

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
                tiles.Add(GetBiome(heightMap[x, y]));
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
