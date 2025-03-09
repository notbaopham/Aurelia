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

    // Patrol boundary points.
    public Transform leftPoint;
    public Transform rightPoint;

    // The child object that holds Rigidbody2D, Animator, SpriteRenderer, and Health.
    public Transform Body;
    private Rigidbody2D bodyRb;
    private Animator animator;
    private Health healthScript; // Reference to the Health component

    private bool movingRight = true;
    private Vector3 leftWorld;
    private Vector3 rightWorld;

    void Start()
    {
        // Cache the world positions so that they remain fixed.
        leftWorld = leftPoint.position;
        rightWorld = rightPoint.position;

        // Set starting position to left boundary.
        Body.position = leftWorld;

        // Get components from Body.
        bodyRb = Body.GetComponent<Rigidbody2D>();
        animator = Body.GetComponent<Animator>();
        healthScript = Body.GetComponent<Health>();

        // Initially set isMoving to false.
        animator.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        // If bodyRb has been destroyed, exit early.
        if (bodyRb == null)
            return;

        // If the wolf is hurt or dead, stop movement.
        if (healthScript != null && !healthScript.canMove)
        {
            return;
        }

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
        if (bodyRb == null) return;
        float moveDir = movingRight ? 1f : -1f;
        Vector2 movement = new Vector2(moveDir * moveSpeed * Time.fixedDeltaTime, 0f);
        bodyRb.MovePosition(bodyRb.position + movement);
        CheckBoundaries();

        animator.SetBool("isMoving", true);
    }

    // Movement during dash.
    void DashMovement()
    {
        if (bodyRb == null) return;
        float moveDir = movingRight ? 1f : -1f;
        Vector2 dashMovement = new Vector2(moveDir * dashSpeed * Time.fixedDeltaTime, 0f);
        bodyRb.MovePosition(bodyRb.position + dashMovement);
        CheckBoundaries();

        animator.SetBool("isMoving", true);
    }

    // Manages dash timing.
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

    // Checks the Body's position against the patrol boundaries.
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

    // Update sprite orientation so that the wolf faces the direction it's moving.
    void LateUpdate()
    {
        // Check if Body has been destroyed; if so, return.
        if (Body == null)
        {
            return;
        }

        SpriteRenderer sr = Body.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // When moving right, face right (flipX = false); when moving left, face left (flipX = true).
            sr.flipX = !movingRight;
        }
    }
}
