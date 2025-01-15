using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player properties")]
    public float speed = 5;
    public float jumpForce = 10f;
    public float crouchSpeedMultiplier = 0.35f;
    public float sprintSpeedMultiplier = 1.5f;
    public float groundCheckRadius = 0.1f;
    public string groundTag = "Ground";
    [Header("Super jump properties")]
    public float crouchHoldTime = 1.5f;
    public float superJumpForce = 15f;
    private float jumpStarttime;
    private bool ischarging;
    [Header("Extra crouch properties")]
    public float normalSizeX = 0.6f;
    public float normalSizeY = 1f;
    public float crouchedSizeX = 0.6f;
    public float crouchedSizeY = 0.2f;
    [Header("Player 1")]
    public BoxCollider2D player1Collider;
    public Rigidbody2D player1Rb;
    public Animator player1Animator;
    public Transform p1GroundCheck;
    public SpriteRenderer player1SpriteRenderer;
    private bool p1Crouching;
    private bool p1Sprinting;
    private bool p1SuperJump;
    [Header("Player 2")]
    public BoxCollider2D player2Collider;
    public Rigidbody2D player2Rb;
    public Animator player2Animator;
    public Transform p2GroundCheck;
    public SpriteRenderer player2SpriteRenderer;
    private bool p2Crouching;
    private bool p2Sprinting;
    private bool p2SuperJump;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(p1GroundCheck.position, groundCheckRadius);
        Gizmos.DrawWireSphere(p2GroundCheck.position, groundCheckRadius);
    }

    private void P1Logic()
    {
        // Initialize values
        float horizontalValue = 0;
        float currentSpeed = speed;
        bool isGrounded = IsGrounded(p1GroundCheck);

        // Get horizontal movement
        if (Input.GetKey(KeyCode.A)) { horizontalValue = -1; }
        if (Input.GetKey(KeyCode.D)) { horizontalValue = 1; }
        if (horizontalValue != 0) { player1SpriteRenderer.flipX = Input.GetKey(KeyCode.A); }

        // Set some variables
        p1Crouching = Input.GetKey(KeyCode.S);
        p1Sprinting = Input.GetKey(KeyCode.LeftShift);

        // Set the collider size according to if the player is crouching
        if (p1Crouching)
        {
            player1Collider.size = new Vector2(crouchedSizeX, crouchedSizeY);
        }
        else
        {
            player1Collider.size = new Vector2(normalSizeX, normalSizeY);
        }

        // Set the speed
        if (p1Crouching) { currentSpeed = 0; }
        if (p1Sprinting && !p1Crouching) { currentSpeed *= sprintSpeedMultiplier; }

        // Apply the speed
        Vector2 targetVelocity = new(horizontalValue * currentSpeed * 100 * Time.deltaTime, player1Rb.velocity.y);
        player1Rb.velocity = targetVelocity;

        // Checks for that the player falls slower that it moves up
        if (player1Rb.velocity.y < 0)
        {
            player1Rb.velocity += 2f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Faster jump
        }
        else if (player1Rb.velocity.y > 0 && !Input.GetKey(KeyCode.W))
        {
            player1Rb.velocity += 1f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Slower fall
        }

        // Super jump logic
        if (p1Crouching && isGrounded)
        {
            if (!ischarging)
            {
                jumpStarttime = Time.time + crouchHoldTime;
                ischarging = true;
            }

            if (ischarging && Time.time > jumpStarttime)
            {
                p1SuperJump = true;
            }
        }
        else
        {
            jumpStarttime = 0;
        }

        // Check if player can jump
        if (Input.GetKey(KeyCode.W) && isGrounded)
        {
            float force = jumpForce;
            // If it is a super jump, use that force instead
            if (p1SuperJump) { force = superJumpForce; p1SuperJump = false; }
            player1Rb.velocity = new Vector2(player1Rb.velocity.x, force);
        }
    }

    private void P2Logic()
    {
        // Initialize values
        float horizontalValue = 0;
        float currentSpeed = speed;
        bool isGrounded = IsGrounded(p2GroundCheck);

        // Get horizontal movement
        if (Input.GetKey(KeyCode.LeftArrow)) { horizontalValue = -1; }
        if (Input.GetKey(KeyCode.RightArrow)) { horizontalValue = 1; }
        if (horizontalValue != 0) { player2SpriteRenderer.flipX = Input.GetKey(KeyCode.LeftArrow); }

        // Set some variables
        p2Crouching = Input.GetKey(KeyCode.DownArrow);

        // Set the collider size according to if the player is crouching
        if (p2Crouching)
        {
            player2Collider.size = new Vector2(crouchedSizeX, crouchedSizeY);
        }
        else
        {
            player2Collider.size = new Vector2(normalSizeX, normalSizeY);
        }

        p2Sprinting = Input.GetKey(KeyCode.RightShift);

        // Set the speed
        if (p2Crouching) { currentSpeed *= crouchSpeedMultiplier; }
        if (p2Sprinting && !p2Crouching) { currentSpeed *= sprintSpeedMultiplier; }

        // Apply the speed
        Vector2 targetVelocity = new(horizontalValue * currentSpeed * 100 * Time.deltaTime, player2Rb.velocity.y);
        player2Rb.velocity = targetVelocity;

        // Checks for that the player falls slower that it moves up
        if (player2Rb.velocity.y < 0)
        {
            player2Rb.velocity += 2f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Faster jump
        }
        else if (player2Rb.velocity.y > 0 && !Input.GetKey(KeyCode.UpArrow))
        {
            player2Rb.velocity += 1f * Physics2D.gravity.y * Time.deltaTime * Vector2.up; // Slower fall
        }

        // Check if player can jump
        if (Input.GetKey(KeyCode.UpArrow) && isGrounded)
        {
            float force = jumpForce;
            player2Rb.velocity = new Vector2(player2Rb.velocity.x, force);
        }
    }

    // Check if the player is grounded by getting items colliding with the groundcheck gameobj,
    // and if there are any objects that have the tag groundCheck in that, its grounded.
    private bool IsGrounded(Transform groundCheck)
    {
        ContactFilter2D filter = new ContactFilter2D();
        Collider2D[] results = new Collider2D[10];

        Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, filter, results);

        foreach (var collider in results)
        {
            if (collider != null && collider.CompareTag(groundTag))
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        // Call the player logic functions
        P1Logic();
        P2Logic();
    }
}
