using System.Collections.Generic;
using UnityEngine;

public class GenerateMeshChunks : MonoBehaviour
{
    public Transform player;

    public static Vector2 playerPosition;

    [Header("Dimensions")]
    public int chunkSize;
    public int maxLoadDistance;
    int chunksVisibleInLoadDistance;

    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksVisibleLastUpdate = new List<TerrainChunk>();

    void Start()
    {
        chunksVisibleInLoadDistance = Mathf.RoundToInt(maxLoadDistance / chunkSize);
    }

    private void Update()
    {
        playerPosition = new Vector2(player.position.x,player.position.z);
        UpdateVisibleChunks();
    }
    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksVisibleLastUpdate.Count; i++)
        {
            terrainChunksVisibleLastUpdate[i].SetVisible(false);
        }
        terrainChunksVisibleLastUpdate.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(playerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(playerPosition.y / chunkSize);

        for (int x = -chunksVisibleInLoadDistance; x < chunksVisibleInLoadDistance; x++)
        {
            for (int y = -chunksVisibleInLoadDistance; y < chunksVisibleInLoadDistance; y++)
            {
                Vector2 chunkCoord = new Vector2(currentChunkCoordX + x, currentChunkCoordY + y);

                if (terrainChunkDictionary.ContainsKey(chunkCoord))
                {
                    terrainChunkDictionary[chunkCoord].UpdateTerrainChunk();

                    if (terrainChunkDictionary[chunkCoord].isVisible())
                    {
                        terrainChunksVisibleLastUpdate.Add(terrainChunkDictionary[chunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(chunkCoord, new TerrainChunk(chunkCoord, chunkSize, maxLoadDistance));
                }
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        int loadDistance;
        Bounds bounds;

        public TerrainChunk(Vector2 coord, int size, int maxLoadDistance)
        {
            loadDistance = maxLoadDistance;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = GameObject.CreatePrimitive(PrimitiveType.Plane);
            meshObject.transform.position = positionV3;
            meshObject.transform.localScale = Vector3.one * size/ 10f;

            SetVisible(false);
        }

        public void UpdateTerrainChunk()
        {
            float playerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(playerPosition));
            bool visible = playerDistanceFromNearestEdge <= loadDistance;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool isVisible()
        {
            return meshObject.activeSelf;
        }
    }
}
