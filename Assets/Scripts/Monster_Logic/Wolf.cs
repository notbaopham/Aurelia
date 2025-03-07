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

    // Patrol boundary points (these can be children of the wolf)
    public Transform leftPoint;
    public Transform rightPoint;

    // The actual sprite that should move (child of wolf)
    public Transform Body;

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
    }

    void Update()
    {
        // Choose between normal movement and dash movement for the Body.
        if (!isDashing)
        {
            MoveBody();
        }
        else
        {
            DashMovement();
        }

        UpdateDashTimer();
    }

    // Normal movement for the Body.
    void MoveBody()
    {
        float moveDirection = movingRight ? 1f : -1f;
        Body.Translate(Vector3.right * moveDirection * moveSpeed * Time.deltaTime, Space.World);
        CheckBoundaries();
    }

    // Movement during dash.
    void DashMovement()
    {
        Vector3 dashDirection = movingRight ? Vector3.right : Vector3.left;
        Body.Translate(dashDirection * dashSpeed * Time.deltaTime, Space.World);
        CheckBoundaries();
    }

    // Manages the dash timing.
    void UpdateDashTimer()
    {
        dashTimer += Time.deltaTime;

        if (dashTimer >= dashInterval && !isDashing)
        {
            isDashing = true;
            dashTimeLeft = dashDuration;
            dashTimer = 0f;
        }

        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
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

    //if the Body is hit by a player attack.
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
