using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    // The radius of the patrol circle.
    public float patrolRadius = 2f;

    // Angular speed in degrees per second.
    public float angularSpeed = 90f;

    // The center point of the patrol circle (set at start from the Body).
    private Vector3 centerPoint;
    private float angle;

    // Reference to the Body object that will move (child of FlyingMonster).
    public Transform Body;
    private Rigidbody2D bodyRb;
    private Animator animator;

    // Health component reference (which controls canMove).
    private Health healthScript;

    void Start()
    {
        // Cache components from the Body.
        bodyRb = Body.GetComponent<Rigidbody2D>();
        animator = Body.GetComponent<Animator>();
        healthScript = Body.GetComponent<Health>();

        // Set the center point as the Body's starting position.
        centerPoint = Body.position;

        // Begin in flight state.
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
    }

    void FixedUpdate()
    {
        // If bodyRb has been destroyed, exit early.
        if (bodyRb == null)
            return;
        // If the monster is hurt or dead (cannot move), stop updating movement.
        if (healthScript != null && !healthScript.canMove)
        {
            if (animator != null)
            {
                animator.SetBool("isMoving", false);
            }
            return;
        }

        Patrol();
    }

    void Patrol()
    {
        if (bodyRb == null) return;
        // Increase the angle based on angular speed and fixedDeltaTime.
        angle += angularSpeed * Time.fixedDeltaTime;
        float rad = angle * Mathf.Deg2Rad;

        // Calculate the offset from the center.
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * patrolRadius;

        // Calculate the new position.
        Vector2 newPos = centerPoint + offset;

        // Move the Body using Rigidbody2D's MovePosition.
        bodyRb.MovePosition(newPos);

        // Ensure the Flight animation is active.
        if (animator != null)
        {
            animator.SetBool("isMoving", true);
        }
    }
}
