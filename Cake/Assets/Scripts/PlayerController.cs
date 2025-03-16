using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  
    public float jumpForce = 7f; 
    public float invincibleTime = 1.5f;  
    public float fanForce = 7f;   

    private bool isOnFan = false; 
    private bool isInvincible = false; 

    private Rigidbody2D rb;
    private bool isGrounded;      
    private SpriteRenderer spriteRenderer; 
    private GameManager gameManager; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); 
        spriteRenderer = GetComponent<SpriteRenderer>();  
        gameManager = FindFirstObjectByType<GameManager>(); 
    }

    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); 
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput < 0) spriteRenderer.flipX = true;
        else if (moveInput > 0) spriteRenderer.flipX = false;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        if (isOnFan)
        {
            rb.AddForce(Vector2.up * fanForce, ForceMode2D.Force); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))  
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("apple"))
        {
            gameManager.CollectApple(other.gameObject);
        }
        else if (other.CompareTag("saw") && !isInvincible)
        {
            gameManager.ReduceLife(1);
            StartCoroutine(Invincibility());
        }
        else if (other.CompareTag("fan"))
        {
            isOnFan = true; 
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fanForce);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("fan"))
        {
            isOnFan = false; 
        }
    }

    private IEnumerator Invincibility()
    {
        isInvincible = true;
        float flashInterval = 0.1f;
        int flashCount = Mathf.RoundToInt(invincibleTime / flashInterval); 

        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSecondsRealtime(flashInterval);
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
