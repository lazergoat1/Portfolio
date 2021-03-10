using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rb2D;
    public Collider2D collider;
    public float dmg;
    public float speed;
    public float bulletLifetime;

    private void Awake()
    {
        rb2D = this.GetComponent<Rigidbody2D>();
    }

    public void applyForce(Vector2 direction)
    {
        float newDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        rb2D.rotation = newDirection;
        rb2D.AddForce(direction * speed * Time.deltaTime, ForceMode2D.Impulse);

        Destroy(this.gameObject, bulletLifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<BasePlayer>().TakeDmg(dmg);
            Destroy(gameObject);
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Enemy>().TakeDmg(dmg);
            Destroy(gameObject);
        }
    }
}
