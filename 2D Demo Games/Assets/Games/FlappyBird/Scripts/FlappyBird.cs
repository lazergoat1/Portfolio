using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlappyBird : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpForce;
    public float moveSpeed;
    public KeyCode jump;
    public float SetJumpTimer;

    float jumpTimer;

    private void Update()
    {
        jumpInput();
        JumpTimer();
        Move();
        RestartLevel();
    }

    void RestartLevel()
    {
        if(rb.transform.position.y < -5f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void jumpInput()
    {
        if(Input.GetKeyDown(jump))
        {
            JumpAction();
        }
    }

    public void JumpAction()
    {
        if (jumpTimer <= 0)
        {
            jumpTimer = SetJumpTimer;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        } 
    }

    void JumpTimer()
    {
        if(jumpTimer >= 0)
        {
            jumpTimer -= 1 * Time.deltaTime;
        }
    }
    public void Move()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }
}
