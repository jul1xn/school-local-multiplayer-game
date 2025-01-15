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
    [Header("Super jump properties")]
    public float crouchHoldTime = 1.5f;
    public float superJumpForce = 15f;
    private float jumpStarttime;
    private bool ischarging;
    [Header("Player 1")]
    public Rigidbody2D player1Rb;
    public Animator player1Animator;
    public Transform p1GroundCheck;
    public SpriteRenderer player1SpriteRenderer;
    private bool p1Crouching;
    private bool p1Sprinting;
    private bool p1SuperJump;

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(p1GroundCheck.position, groundCheckRadius);
        //Gizmos.DrawWireSphere(p1GroundCheck.position, groundCheckRadius);
    }

    private void Player1Logic()
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

        // Set the speed
        if (p1Crouching) { currentSpeed *= crouchSpeedMultiplier; }
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

    private bool IsGrounded(Transform groundCheck)
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius) != null;
    }

    private void Update()
    {
        Player1Logic();
    }
}
