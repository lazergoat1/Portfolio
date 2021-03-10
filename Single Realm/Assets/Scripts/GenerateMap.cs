using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GenerateMap : MonoBehaviour
{
    public List<int> xPos;
    public List<int>  yPos;

    public Tilemap tilemap;
    public TileBase[] tilesToChooseFrom;


    private void Start()
    {
        CreateTiles();
    }

    private void CreateTiles()
    {
        for (int a = 0; a < xPos.Count; a++)
        {
            for (int b = 0; b < yPos.Count; b++)
            {
                tilemap.SetTile(new Vector3Int(a, b, (int)tilemap.transform.position.z), tilesToChooseFrom[0]);
            }
        }
    }
}
