using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public PlayerStats playerStats;

    GameObject player;

    public HealthBar healthBar;
    public float maxHealth = 100f;
    public float speed;
    public float shootDelay;
    public float newMovePointDelay;
    public float range;

    public float givenXp;

    public float wanderDistanceX;
    public float wanderDistanceY;

    float health;
    float _shootDelay;
    float _newMovePointDelay;
    bool shoot;

    Vector2 movePoint;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        CheckForPlayer();
    }

    private void FixedUpdate()
    {
        AimlessMove();
        if (player)
        {
            animator.SetBool("isAttacking", true);
            Shoot(player.transform);
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }
    }

    void CheckForPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach(Collider2D character in colliders)
        {
            if(character.tag == "Player")
            {
                
                player = character.gameObject;
                break;
            }
            else
            {
                player = null;
            }
        }
    }

    void AimlessMove()
    {
        _newMovePointDelay -= Time.deltaTime;

        if (_newMovePointDelay <= 0)
        {
            movePoint = new Vector2(transform.position.x + Random.Range(-wanderDistanceX, wanderDistanceX), transform.position.y + Random.Range(-wanderDistanceY, wanderDistanceY));

            _newMovePointDelay = newMovePointDelay;
        }

        if (movePoint != null)
        {
            transform.position = Vector2.MoveTowards(rb.position, movePoint, speed * Time.deltaTime);
        }

        Vector2 moveDirection = (movePoint - (Vector2)transform.position).normalized;

        if (!animator.GetBool("isAttacking"))
        {
            if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (moveDirection.x < 0)
            {
                spriteRenderer.flipX = false;
            }

            animator.SetFloat("vertical", moveDirection.y);
            animator.SetFloat("horizontal", moveDirection.x);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

    }

    void Rotate(Transform targetToLookAt)
    {
        Vector2 lookDir = (Vector2)targetToLookAt.position - rb.position;

        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;

        if (angle < -180 || angle > 0 && angle < 90)
        {
            spriteRenderer.flipX = false;
        }
        else if (angle > -180 && angle < 0)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat("angle", angle);
    }


    void Shoot(Transform target)
    {
        _shootDelay -= Time.deltaTime;

        if (_shootDelay <= 0f)
        {
                Rotate(target);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Vector2 direction = ((Vector2)target.position - (Vector2)bullet.transform.position).normalized;
            bullet.GetComponent<Projectile>().applyForce(direction);

            _shootDelay = shootDelay;
        }
    }

    public void TakeDmg(float dmg)
    {
        if (health > 0)
        {
            health -= dmg;
            Popup.Create(transform.position, (int)dmg, GameAssets.instance.damagePopup);
            healthBar.ChangeSize(maxHealth, health);
            if (health <= 0)
            {
                kill();
            }
        }
    }

    public void kill()
    {
        playerStats.xp += givenXp;
        Popup.Create(GameAssets.instance.player.transform.position, (int)givenXp, GameAssets.instance.xpPopup);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

}

