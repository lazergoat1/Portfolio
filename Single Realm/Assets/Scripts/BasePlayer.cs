using UnityEngine;
using UnityEngine.SceneManagement;

public class BasePlayer : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public ParticleSystem rollParticles;

    public Rigidbody2D rb;
    public Camera camera;

    public PlayerStats playerHealth;
    public HealthBar healthBar;

    public Transform firePoint;
    public GameObject bulletPrefab;

    public float shootDelay;

    public float speed;
    public float rollSpeed;
    public float rollLength;
    public float rollDelay;
    public KeyCode rollKey;

    public float shootDistanceFromPlayer;

    private bool shoot;
    private bool roll;
    private bool rolling;
    private float _shootDelay;
    private float rollTime;
    private float _rollDelay;
    private float moveDirectionVertical = 1f;
    private float moveDirectionHorizontal = 1f;

    private void Update()
    {
        if(Input.GetKey(rollKey))
        {
            roll = true;
        }
        
        if(!rolling)
        {
            if (Input.GetButton("Fire1"))
            {
                if (_shootDelay <= 0)
                {
                    shoot = true;
                }
                animator.SetBool("isAttacking", true);
            }
            else
            {
                animator.SetBool("isAttacking", false);
            }
        }
    }

    void FixedUpdate()
    {
        if(!rolling)
        {
            Move();
            Shoot();
        }

        Roll();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float moveDirection = new Vector2(horizontal, vertical).magnitude;

        if (moveDirection > 0)
        {
            if(!animator.GetBool("isAttacking"))
            {
                if (horizontal > 0)
                {
                    spriteRenderer.flipX = true;
                }
                else if (horizontal < 0)
                {
                    spriteRenderer.flipX = false;
                }

                animator.SetFloat("vertical", vertical);
                animator.SetFloat("horizontal", horizontal);
                animator.SetBool("isMoving", true);
            }

            moveDirectionHorizontal = horizontal;
            moveDirectionVertical = vertical;

        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        Vector3 velocity = new Vector3(horizontal, vertical, 0).normalized;
        rb.velocity = velocity * speed * Time.deltaTime;
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

        // Need to lock it within a circle 
        // you can use this to change the roll rotation
       // firePoint.position = ((mousePos - (Vector2)firePoint.position).normalized * shootDistanceFromPlayer) + (Vector2)transform.position;

        animator.SetFloat("angle", angle);
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

    private void Roll()
    {
        if (roll)
        {
            if(_rollDelay <= 0f)
            {
                roll = false;
                rolling = true;
                rollTime = rollLength;
                _rollDelay = rollDelay;
            }
            else
            {
                roll = false;
            }
        }

        if (rolling)
        {
            rollTime -= Time.deltaTime;

            rollParticles.Play();
            Vector3 velocity = new Vector3(moveDirectionHorizontal, moveDirectionVertical, 0).normalized;

            rb.velocity = velocity * rollSpeed * Time.deltaTime;
        }

        if (rollTime <= 0)
        {
            rolling = false;
        }

        _rollDelay -= Time.deltaTime;
    }

    public void TakeDmg(float dmg)
    {
        playerHealth.currentHealth -= dmg;
        Popup.Create(transform.position, (int)dmg, GameAssets.instance.damagePopup);
        healthBar.ChangeSize(playerHealth.health, playerHealth.currentHealth);

        if (playerHealth.currentHealth <= 0)
        {
            kill();
        }
    }

    public void kill()
    {
        //Play kill animation 
        //Load restart screen
        playerHealth.currentHealth = playerHealth.health;
        SceneManager.LoadScene(Random.Range(0, SceneManager.sceneCountInBuildSettings));
    }
}
