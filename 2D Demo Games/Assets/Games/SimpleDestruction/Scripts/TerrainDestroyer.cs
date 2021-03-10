using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainDestroyer : MonoBehaviour
{
    public Tilemap terrain;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Bullet"))
        {
            Vector3 hitPos = Vector3.zero;

            foreach(ContactPoint2D hit in collision.contacts)
            {
                hitPos.x = hit.point.x -0.001f * hit.normal.x;
                hitPos.y = hit.point.y - 0.001f * hit.normal.y;

                terrain.SetTile(terrain.WorldToCell(hitPos), null);
            }
            Destroy(collision.gameObject);
        }
    }
}
