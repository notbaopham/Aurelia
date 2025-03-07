using UnityEngine;

public class Insect : MonoBehaviour
{
    // The radius of the patrol circle.
    public float patrolRadius = 2f;

    // Angular speed in degrees per second.
    public float angularSpeed = 90f;

    // The center point of the patrol circle (set at start).
    private Vector3 centerPoint;

    // The current angle (in degrees) used to compute position on the circle.
    private float angle;

    void Start()
    {
        // Set the center point as the insect's starting position.
        centerPoint = transform.position;
    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        // Increase the angle based on angular speed and elapsed time.
        angle += angularSpeed * Time.deltaTime;

        // Convert the angle to radians for trigonometric functions.
        float rad = angle * Mathf.Deg2Rad;

        // Calculate the offset from the center using cosine for X and sine for Y.
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0f) * patrolRadius;

        // Set the insect's position to the center plus the offset.
        transform.position = centerPoint + offset;
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
