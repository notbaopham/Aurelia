using UnityEngine;
using UnityEngine.InputSystem;

// TODO:
/*
    Slide - on pressing Shift - lowering Collision box, and adds a temporary velocity boost
    Dash - only applicable in the air - propels where the character is facing
    Attack - E/LeftClick, creates a cascading cone hitbox infront of player
    Health System - player will be having a hp system
*/
public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    [SerializeField] public float moveSpeed = 5f;

    Vector2 moveDirection = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Things to do - convert this whole code into the new InputSystem
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        rb.linearVelocity = new Vector2(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        // Barebone procedure of Jump key - make sure that you fix up this code asap
        if (Input.GetKey(KeyCode.Space))
            Jump();
    }

    void Jump() {
        rb.linearVelocityY = moveSpeed;
    }


    private void FixedUpdate()
    {

    }
}
