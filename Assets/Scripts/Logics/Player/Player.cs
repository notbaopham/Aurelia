using System;
using NUnit.Framework;
using UnityEngine;

// TODO:
/*
    Slide - on pressing Shift - lowering Collision box, and adds a temporary velocity boost
    Dash - only applicable in the air - propels where the character is facing
    Attack - E/LeftClick, creates a cascading cone hitbox infront of player
    Health System - player will be having a hp system
*/
public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;
    public Rigidbody2D rb;
    private void Awake()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(Jump);
        rb = GetComponent<Rigidbody2D>();
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
        Vector2 velocity = rb.linearVelocity;
        
        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }

        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
    }

    private bool isTouchingGround() 
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1.1f, 3);
    }
    
    void Jump() {
        // Need to ask TA / Teacher on why the horizontal momentum are not being maintained
        Vector2 jumpDir = Vector2.up;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
    }

    private void MovePlayer(Vector2 directionInput) {
        rb.AddForce(directionInput * acceleration);
    }

    public float softLandingForce = 10f; // Small force to counteract the abrupt landing

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground") && rb.linearVelocity.y < 0) // Only apply on landing
        {
            // Slight upward force to smooth landing
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Lerp(rb.linearVelocity.y, 0, Time.deltaTime * softLandingForce));
        }
    }
}
        
