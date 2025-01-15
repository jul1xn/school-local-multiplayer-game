using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public Fields
    public float speed = 5;
    public float jumpForce = 10f;
    public float superJumpForce = 15f;
    public Sprite defaultSprite;
    public Sprite crouchSprite;
    public Sprite jumpSprite;
    public Sprite landSprite;
    public float crouchHoldTime = 1.5f;
    public float crouchSpeedMultiplier = 0.35f;
    public float maxSuperJumpTime = 5f;
    public float sprintSpeedMultiplier = 1.5f;

    // Private Fields
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;
    private SpriteRenderer spriteRenderer;
    private float horizontalValue;
    private bool facingRight = true;
    private bool isCrouching = false;
    private bool isJumping = false;
    private bool superJumpReady = false;
    private float crouchTimer = 0f;
    private float superJumpTimer = 0f;

    // Original collider size and offset
    private Vector2 originalColliderSize;
    private Vector2 originalColliderOffset;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Save original collider size and offset
        originalColliderSize = playerCollider.size;
        originalColliderOffset = playerCollider.offset;
    }

    void Update()
    {
        horizontalValue = Input.GetAxisRaw("Horizontal");
        HandleCrouch();
        HandleJump();
    }

    void FixedUpdate()
    {
        float currentSpeed = isCrouching ? speed * crouchSpeedMultiplier : speed;
        Move(horizontalValue, currentSpeed);

        // Handle faster falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += 2f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Faster fall
        }
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            rb.velocity += 1f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Slower jump ascent
        }

        // Check if landed
        CheckLanding();
    }

    void Move(float dir, float moveSpeed)
    {
        // Check if Shift is held for sprinting
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed *= sprintSpeedMultiplier;
        }

        float xVal = dir * moveSpeed * 100 * Time.deltaTime;
        Vector2 targetVelocity = new(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        // Flip player direction
        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            facingRight = true;
        }
    }

    void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.S))
        {
            crouchTimer += Time.deltaTime;
            if (crouchTimer >= crouchHoldTime && !isCrouching)
            {
                StartCrouching();
            }
        }
        else
        {
            if (crouchTimer >= crouchHoldTime)
            {
                superJumpReady = true;
                superJumpTimer = maxSuperJumpTime; // Set timer for super jump activation
            }

            crouchTimer = 0f;

            if (isCrouching)
            {
                StopCrouching();
            }
        }

        // Count down the super jump timer
        if (superJumpReady)
        {
            superJumpTimer -= Time.deltaTime;
            if (superJumpTimer <= 0f)
            {
                superJumpReady = false; // Expire super jump window
            }
        }
    }

    void StartCrouching()
    {
        isCrouching = true;
        spriteRenderer.sprite = crouchSprite; // Change to crouch sprite
        AdjustColliderToSprite(crouchSprite);

        Debug.Log("Crouching: Collider adjusted.");
    }

    void StopCrouching()
    {
        isCrouching = false;
        spriteRenderer.sprite = defaultSprite; // Change back to default sprite
        ResetColliderToDefault();

        Debug.Log("Stopped Crouching: Collider reset.");
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            if (superJumpReady)
            {
                Jump(superJumpForce); // Perform super jump
                superJumpReady = false; // Consume super jump
            }
            else
            {
                Jump(jumpForce); // Perform normal jump
            }
        }
    }

    void Jump(float force)
    {
        isJumping = true;
        rb.velocity = new Vector2(rb.velocity.x, force);
        spriteRenderer.sprite = jumpSprite; // Change to jump sprite
        Debug.Log("Jumping with force: " + force);
    }

    void AdjustColliderToSprite(Sprite sprite)
    {
        Bounds spriteBounds = sprite.bounds;

        // Adjust collider size and offset based on sprite bounds
        playerCollider.size = new Vector2(spriteBounds.size.x, spriteBounds.size.y);
        playerCollider.offset = new Vector2(spriteBounds.center.x, spriteBounds.center.y);
    }

    bool IsGrounded()
    {
        // Check if the player is on the ground
        return rb.velocity.y == 0;
    }

    void CheckLanding()
    {
        if (isJumping && IsGrounded())
        {
            isJumping = false;
            spriteRenderer.sprite = landSprite; // Set to land sprite
            StartCoroutine(WaitAndResetSprite()); // Start the coroutine to wait and reset the sprite

            // Reset the collider size and offset to match the land sprite
            AdjustColliderToSprite(landSprite);  // Adjust collider size just like when crouching
            Debug.Log("Landed: Sprite and collider adjusted to land.");
        }
    }

    IEnumerator WaitAndResetSprite()
    {
        // Wait for 1 second before resetting the sprite
        yield return new WaitForSeconds(0.35f);

        // Reset sprite to default after 1 second
        spriteRenderer.sprite = defaultSprite;
    }

    void ResetColliderToDefault()
    {
        // Reset the collider size and offset to the original values
        playerCollider.size = originalColliderSize;
        playerCollider.offset = originalColliderOffset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with the ground (adjust your tag or layer name as needed)
        if (collision.gameObject.CompareTag("Ground") && isJumping)
        {
            // Landed, reset the jump flag
            CheckLanding();
        }
    }
}
