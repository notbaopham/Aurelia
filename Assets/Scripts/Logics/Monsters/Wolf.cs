using UnityEngine;

public class Wolf : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float dashSpeed = 5f;
    public float dashInterval = 2f;
    private float dashTimer;
    private bool isDashing = false;
    private float dashDuration = 0.2f; // Duration of the dash
    private float dashTimeLeft;

    // Patrol boundary points 
    public Transform leftPoint;
    public Transform rightPoint;

    // The actual sprite that should move (child of wolf) with a Rigidbody2D component.
    public Transform Body;
    private Rigidbody2D bodyRb;

    private bool movingRight = true;
    private Vector3 leftWorld;
    private Vector3 rightWorld;

    void Start()
    {
        // Cache the world positions so that they remain fixed even if the parent moves.
        leftWorld = leftPoint.position;
        rightWorld = rightPoint.position;

        // Set the Body's starting position to the left boundary.
        Body.position = leftWorld;

        // Get the Rigidbody2D component from the Body.
        bodyRb = Body.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        UpdateDashTimer();

        if (!isDashing)
        {
            MoveBody();
        }
        else
        {
            DashMovement();
        }
    }

    // Normal movement.
    void MoveBody()
    {
        float moveDirection = movingRight ? 1f : -1f;
        Vector2 movement = new Vector2(moveDirection * moveSpeed * Time.fixedDeltaTime, 0f);
        bodyRb.MovePosition(bodyRb.position + movement);
        CheckBoundaries();
    }

    // Movement during dash.
    void DashMovement()
    {
        float moveDirection = movingRight ? 1f : -1f;
        Vector2 dashMovement = new Vector2(moveDirection * dashSpeed * Time.fixedDeltaTime, 0f);
        bodyRb.MovePosition(bodyRb.position + dashMovement);
        CheckBoundaries();
    }

    // Manages the dash timing.
    void UpdateDashTimer()
    {
        dashTimer += Time.fixedDeltaTime;

        if (dashTimer >= dashInterval && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashTimer = 0f;
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.fixedDeltaTime;
            if (dashTimeLeft <= 0f)
            {
                isDashing = false;
            }
        }
    }

    // Checks the Body's position against the cached world boundaries.
    void CheckBoundaries()
    {
        if (Body.position.x >= rightWorld.x)
        {
            movingRight = false;
        }
        else if (Body.position.x <= leftWorld.x)
        {
            movingRight = true;
        }
    }

    // Detect player attack with trigger collider.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
