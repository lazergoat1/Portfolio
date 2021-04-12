using UnityEngine;
using UnityEngine.SceneManagement;

public class BasePlayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public static GameObject player;

    public Rigidbody2D rb;
    private Camera camera;

    public PlayerStats playerStats;
    public HealthBar healthBar;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float shootDelay;

    public float speed;

    private bool shoot;
    private float _shootDelay;

    private void Start()
    {
        healthBar.ChangeSize(playerStats.health, playerStats.currentHealth);
        camera = Camera.main;
    }
    private void Update()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, camera.transform.position.z);

        if (Input.GetButton("Fire1"))
        {
            if (_shootDelay <= 0)
            {
                shoot = true;
            }
        }
    }

    void FixedUpdate()
    {
        Move();
        Shoot();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0 && !Input.GetButton("Fire1"))
        {
            spriteRenderer.flipX = true;
        }
        else if(horizontal < 0 && !Input.GetButton("Fire1"))
        {
            spriteRenderer.flipX = false;
        }

        Vector3 velocity = new Vector3(horizontal, vertical, 0).normalized;
        rb.velocity = velocity * speed * Time.deltaTime;
        playerStats.position = transform.position;
    }

    void Rotate()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if (angle < -180|| angle > 0 && angle < 90)
        {
            spriteRenderer.flipX = false;
        }
        else if (angle > -180 && angle < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    void Shoot()
    {
        _shootDelay -= Time.deltaTime;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (shoot)
        {
            Rotate();

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Vector2 direction = (mousePos - (Vector2)bullet.transform.position).normalized;
            bullet.GetComponent<Projectile>().applyForce(direction);

            shoot = false;
            _shootDelay = shootDelay;
        }    
    }

    public void TakeDmg(float dmg)
    {
        playerStats.currentHealth -= dmg;
        Popup.Create(transform.position, (int)dmg, GameAssets.instance.damagePopup);
        healthBar.ChangeSize(playerStats.health, playerStats.currentHealth);

        if (playerStats.currentHealth <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        playerStats.currentHealth = playerStats.health;
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}
