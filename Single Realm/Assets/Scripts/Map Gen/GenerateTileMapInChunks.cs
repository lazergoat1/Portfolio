using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateTileMapInChunks : MonoBehaviour
{
    public GameObject playerPrefab;
    public WorldValues mapValues;
    public Tilemap landTilemap;
    public Tilemap waterTilemap;
    public Transform mapObjectsParent;
    public Transform enemiesParent;
    public BiomePreset[] biomes;

    private Transform player;

    [Header("Dimensions")]
    [Range(0,1)]
    public float playerSpawnHeight;
    public int chunkSize;
    public int maxLoadDistance;
    public int respawnDistanceInChunks;

    float[,] heightMap;
    int chunksVisibleInLoadDistance;

    List<Vector2> chunkCoordList = new List<Vector2>();
    Dictionary<Vector2, ChunkValues> chunkValues = new Dictionary<Vector2, ChunkValues>();

    void Start()
    {
        mapValues = GameObject.FindObjectOfType<WorldValues>();
        chunksVisibleInLoadDistance = Mathf.RoundToInt(maxLoadDistance / chunkSize);
        if(GameManager.instance.worldValues.newSave == true)
        {
            SpawnPlayer();
        }
        else
        {
            LoadPlayer();
        }
    }

    private void Update()
    {
        UpdateVisibleChunks();
    }

    private void OnDrawGizmos()
    {
        foreach (Vector2 terrainChunk in chunkCoordList)
        {
            Debug.DrawLine(terrainChunk * chunkSize, terrainChunk * chunkSize + new Vector2(chunkSize, 0));
            Debug.DrawLine(terrainChunk * chunkSize, terrainChunk * chunkSize + new Vector2(0, chunkSize));
            Gizmos.DrawIcon(terrainChunk * chunkSize + new Vector2(chunkSize, chunkSize) * 0.5f, "Center");
        }
    }

    private void SpawnPlayer()
    {
        int length = 1000;
        float[,] hMap = Noise.GenerateTileMapNoise(length, length, mapValues.seed, 0, 0, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);
        for (int x = 0; x < length; x++)
        {
            for (int y = 0; y < length; y++)
            {
                if (hMap[x, y] > playerSpawnHeight)
                {
                    player = Instantiate(playerPrefab, new Vector3(x, y, 0) + transform.position, Quaternion.identity).transform;
                    return;
                }
            }
        }
    }

    private void LoadPlayer()
    {
        player = Instantiate(playerPrefab, GameManager.instance.playerStats.position, Quaternion.identity).transform;
    }

    void UpdateVisibleChunks()
    {
        int currentChunkCoordX = Mathf.RoundToInt(player.position.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(player.position.y / chunkSize);

        for (int i = 0; i < chunkCoordList.Count; i++)
        {
            if (chunkCoordList[i].x > chunksVisibleInLoadDistance + currentChunkCoordX || chunkCoordList[i].x < -chunksVisibleInLoadDistance + currentChunkCoordX)
            {
                UnloadChunk(chunkCoordList[i]);
            }
            else if (chunkCoordList[i].y > chunksVisibleInLoadDistance + currentChunkCoordY || chunkCoordList[i].y < -chunksVisibleInLoadDistance + currentChunkCoordY)
            {
                UnloadChunk(chunkCoordList[i]);
            }
        }

        List<Vector2> chunkValueKeys = new List<Vector2>(chunkValues.Keys);
        for (int i = 0; i < chunkValueKeys.Count; i++)
        {
            if(chunkValueKeys[i].x > respawnDistanceInChunks + currentChunkCoordX || chunkValueKeys[i].x < -respawnDistanceInChunks + currentChunkCoordX)
            {
                foreach (GameObject entitie in chunkValues[chunkValueKeys[i]].entities)
                {
                    Destroy(entitie);
                }
                chunkValues.Remove(chunkValueKeys[i]);
            }
            else if (chunkValueKeys[i].y > respawnDistanceInChunks + currentChunkCoordY || chunkValueKeys[i].y < -respawnDistanceInChunks + currentChunkCoordY)
            {
                foreach(GameObject entitie in chunkValues[chunkValueKeys[i]].entities)
                {
                    Destroy(entitie);
                }
                chunkValues.Remove(chunkValueKeys[i]);
            }
        }

        for (int x = -chunksVisibleInLoadDistance; x < chunksVisibleInLoadDistance; x++)
        {
            for (int y = -chunksVisibleInLoadDistance; y < chunksVisibleInLoadDistance; y++)
            {
                Vector2 chunkCoord = new Vector2(currentChunkCoordX + x, currentChunkCoordY + y);
                
                if (!chunkCoordList.Contains(chunkCoord))
                {
                    chunkCoordList.Add(chunkCoord);
                    LoadChunk(chunkCoord);
                }
            }
        }
    }

    void LoadChunk(Vector2 chunkCoord)
    {
        System.Random RNG = new System.Random(mapValues.seed + (int)chunkCoord.x + (int)chunkCoord.y);
        heightMap = Noise.GenerateTileMapNoise(chunkSize, chunkSize, mapValues.seed, chunkCoord.x * chunkSize, chunkCoord.y * chunkSize, mapValues.scale, mapValues.octaves, mapValues.persistance, mapValues.lacunarity);

        List<Vector3Int> landTilePositions = new List<Vector3Int>();
        List<Vector3Int> waterTilePositions = new List<Vector3Int>();
        List<TileBase> landTiles = new List<TileBase>();
        List<TileBase> waterTiles = new List<TileBase>();

        for (int x = 0; x < chunkSize; x++)
        {
            for (int y = 0; y < chunkSize; y++)
            {
                int xPos = x + Mathf.RoundToInt(chunkCoord.x * chunkSize);
                int yPos = y + Mathf.RoundToInt(chunkCoord.y * chunkSize);

                BiomePreset biome = GetBiome(Mathf.Clamp01(heightMap[x, y]));
                Vector3Int newTilePosition = new Vector3Int(xPos, yPos, 0);

                if (GetBiome(heightMap[x, y]).name != "Ocean")
                {
                    landTilePositions.Add(newTilePosition);
                    landTiles.Add(GetBiome(heightMap[x, y]).PickRandomTile(RNG));
                }
                else
                {
                    landTilePositions.Add(newTilePosition);
                    landTiles.Add(GetBiome(0.3f).PickRandomTile(RNG));
                    waterTilePositions.Add(newTilePosition);
                    waterTiles.Add(GetBiome(heightMap[x, y]).PickRandomTile(RNG));
                }

                LoadEntities(biome, (Vector3)newTilePosition + new Vector3(0.5f, 0.5f), chunkCoord, RNG);
            }
        }
        landTilemap.SetTiles(landTilePositions.ToArray(), landTiles.ToArray());
        waterTilemap.SetTiles(waterTilePositions.ToArray(), waterTiles.ToArray());
    }

    private void LoadEntities(BiomePreset biome, Vector2 tilePosition, Vector2 chunkCoord, System.Random RNG)
    {
        if (!chunkValues.ContainsKey(chunkCoord))
        {
            if (RNG.NextDouble() > 1 - (biome.enemySpawnPercentage / 100))
            {
                Instantiate(biome.PickRandomPrefab(RNG, biome.enemyPrefabs), tilePosition, Quaternion.identity, mapObjectsParent);
            }

            if (RNG.NextDouble() > 1 - (biome.foliageSpawnPercentage / 100))
            {
                Instantiate(biome.PickRandomPrefab(RNG, biome.foliagePrefabs), tilePosition, Quaternion.identity, mapObjectsParent);
            }
        }
        else
        {
            foreach (GameObject entitie in chunkValues[chunkCoord].entities)
            {
                if (entitie != null)
                    entitie.SetActive(true);
            }
        }
    }

    void UnloadChunk(Vector2 chunkCoord)
    {
        Collider2D[] gameObjectsInChunk = Physics2D.OverlapBoxAll((chunkCoord * chunkSize) + new Vector2(chunkSize, chunkSize) * 0.5f, new Vector2(chunkSize, chunkSize), 0);
        List<Vector3Int> tilePositions = new List<Vector3Int>();
        List<TileBase> tiles = new List<TileBase>();

        if (!chunkValues.ContainsKey(chunkCoord))
        {
            chunkValues.Add(chunkCoord, new ChunkValues());
            foreach (Collider2D gameObjectCollider in gameObjectsInChunk)
            {
                if (gameObjectCollider.tag != "Tilemap" && gameObjectCollider.tag != "Player")
                {
                    gameObjectCollider.gameObject.SetActive(false);
                    chunkValues[chunkCoord].entities.Add(gameObjectCollider.gameObject);
                }
            }
        }
        else
        {
            foreach (GameObject entitie in chunkValues[chunkCoord].entities)
            {
                if (entitie != null)
                    entitie.SetActive(false);
            }
        }

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
        landTilemap.SetTiles(tilePositions.ToArray(), tiles.ToArray());
        waterTilemap.SetTiles(tilePositions.ToArray(), tiles.ToArray());
        chunkCoordList.Remove(chunkCoord);
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

    private class ChunkValues
    {
        public List<GameObject> entities = new List<GameObject>();
    }
}
