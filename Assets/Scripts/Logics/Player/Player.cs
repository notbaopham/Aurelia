using System;
using NUnit.Framework;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Video;

// TODO:
/*
    Slide - on pressing Shift - lowering Collision box, and adds a temporary velocity boost
    Dash - only applicable in the air - propels where the character is facing
    Attack - E/LeftClick, creates a cascading cone hitbox infront of player
    Health System - player will be having a hp system
*/
public class Player : MonoBehaviour
{
    // Managers - top objects to refer to
    [SerializeField] private InputManager inputManager;

    // Movement values
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;

    // Overlord variables
    [SerializeField] private LayerMask consideredGround;
    private bool isOnGround;
    private Rigidbody2D rb;
    private Vector2 currentlyFacing;

    // Dashing variables
    private bool isDashing = false;
    [SerializeField] private float dashSpeed = 20f;
    private float dashTime, lastDashTime;
    [SerializeField] private float dashDuration = 0.1f;

    // Double Jump variables
    private bool canDoubleJump;
    private void Awake()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(Jump);
        inputManager.OnDash.AddListener(StartDash);
        rb = GetComponent<Rigidbody2D>();
        currentlyFacing = Vector2.right;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        /* First implementation
        Vector2 velocity = rb.linearVelocity;
        
        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }
        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        */

        if (Math.Abs(rb.linearVelocityX) > maxSpeed) {
            if (rb.linearVelocityX < 0) {
                rb.linearVelocityX = -maxSpeed;
                currentlyFacing = Vector2.left;
            } else if (rb.linearVelocityX > 0) {
                rb.linearVelocityX = maxSpeed;
                currentlyFacing = Vector2.right;
            }
        } 
        if (isDashing) {
            DashMovement();
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.1f, consideredGround);
        if (hit) {
            isOnGround = true;
            canDoubleJump = true;
        } else {
            isOnGround = false;
        }
    }

    void Jump() {
        if (isOnGround) {
            // Need to ask TA / Teacher on why the horizontal momentum are not being maintained
            Vector2 jumpDir = Vector2.up;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
        } else if (canDoubleJump) {
            Vector2 jumpDir = Vector2.up;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }

    private void MovePlayer(Vector2 directionInput) {
        rb.AddForce(directionInput * acceleration);
    }

    [SerializeField] private float softLandingForce = 10f; // Small force to counteract the abrupt landing

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && rb.linearVelocity.y < 0) // Only apply on landing
        {
            // Slight upward force to smooth landing - bumping out the stops at the landing
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Lerp(rb.linearVelocity.y, 0, Time.deltaTime * softLandingForce));
        }
        if (collision.collider.CompareTag("Wall"))
        {

        }
    }

    void StartDash()
    {
        // Initiate dash and record the time
        isDashing = true;
        dashTime = 0f; // Reset dash duration
        lastDashTime = Time.time; // Set cooldown timer

        // Disable gravity during dash
        rb.gravityScale = 0f; // Disable gravity

    }

    void DashMovement()
    {
        // Dash for the specified duration
        dashTime += Time.deltaTime;

        if (dashTime < dashDuration)
        {
            rb.linearVelocity = currentlyFacing * dashSpeed; // Move in dash direction
        }
        else
        {
            isDashing = false; // End the dash after the duration
            rb.linearVelocity = Vector2.zero; // Stop movement after dash ends

            // Restore gravity after dash ends
            rb.gravityScale = 2f; // Restore original gravity scale
        }
    }

}
       
