using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    public LineRenderer lavaGFX;
    public Transform lavaLeft;
    public Transform lavaRight;
    public GameObject player;
    public Rigidbody2D rb;
    public float speed;

    void FixedUpdate()
    {
        rb.MovePosition(this.transform.position + Vector3.up *speed * Time.deltaTime);
        lavaGFX.enabled = true;
        lavaGFX.SetPosition(0, lavaLeft.position);
        lavaGFX.SetPosition(1, lavaRight.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.gameObject == player)
        {
            FindObjectOfType<AudioManager>().Play("Death");
            SceneManager.LoadScene(0);
        }
    }
}
