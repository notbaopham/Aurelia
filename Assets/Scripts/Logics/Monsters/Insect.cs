using UnityEngine;

public class Insect : MonoBehaviour
{
    // The radius of the patrol circle.
    public float patrolRadius = 2f;

    // Angular speed in degrees per second.
    public float angularSpeed = 90f;

    // The center point of the patrol circle (set at start from the Body).
    private Vector3 centerPoint;

    // The current angle (in degrees) used to compute position on the circle.
    private float angle;

    // Reference to the Body object that will move (child of Insect).
    public Transform Body;

    // Rigidbody2D component attached to the Body.
    private Rigidbody2D bodyRb;

    void Start()
    {
        // Cache the Rigidbody2D component from the Body.
        bodyRb = Body.GetComponent<Rigidbody2D>();

        // Set the center point as the Body's starting position.
        centerPoint = Body.position;
    }

    // Use FixedUpdate for physics-based movement.
    void FixedUpdate()
    {
        Patrol();
    }

    void Patrol()
    {
        // Increase the angle based on angular speed and fixedDeltaTime.
        angle += angularSpeed * Time.fixedDeltaTime;

        // Convert the angle to radians.
        float rad = angle * Mathf.Deg2Rad;

        // Calculate the offset from the center using cosine for X and sine for Y.
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * patrolRadius;

        // Calculate the new position.
        Vector2 newPos = centerPoint + offset;

        // Move the Body using Rigidbody2D's MovePosition.
        bodyRb.MovePosition(newPos);
    }

    // Detect collisions with player attacks.
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
