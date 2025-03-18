using UnityEngine;

public class Slime : MonoBehaviour
{
    // Movement parameters
    public float hopForce = 5f;      // Vertical force of the hop
    public float moveSpeed = 2f;     // Horizontal push during the hop
    public float hopInterval = 3f;   // Time between hops

    private float hopTimer;
    private Rigidbody2D rb;
    private Animator animator;
    private float moveDirection;

    // Reference to the child object (Body) that contains Rigidbody2D, Animator, etc.
    public Transform Body;

    // Patrol boundary points (set in the Inspector)
    public Transform leftPoint;
    public Transform rightPoint;
    private Vector3 leftWorld;
    private Vector3 rightWorld;

    // Stores the horizontal direction used for the last hop
    private float lastDirection;

    void Start()
    {
        // Cache the world positions of the boundary points.
        leftWorld = leftPoint.position;
        rightWorld = rightPoint.position;

        // Get the Rigidbody2D and Animator from the Body object.
        rb = Body.GetComponent<Rigidbody2D>();
        animator = Body.GetComponent<Animator>();

        // Choose an initial horizontal direction.
        ChooseNewDirection();
    }

    void Update()
    {
        // Stop executing if the object is being destroyed
        if (rb == null)
        {
            return;
        }

        // Increase the hop timer.
        hopTimer += Time.deltaTime;

        // When the timer exceeds the hop interval, perform a hop.
        if (hopTimer >= hopInterval)
        {
            Hop();
            hopTimer = 0f;
            ChooseNewDirection();
        }

        // Determine whether the slime is airborne based on its vertical velocity.
        bool inAir = Mathf.Abs(rb.linearVelocity.y) > 0.1f;
        // Set the animator parameter so that:
        // - When in the air (inAir true), the Moving animation plays.
        // - When on the ground (inAir false), the Idle animation plays.
        animator.SetBool("isMoving", inAir);
    }

    // Applies an impulse force for hopping.
    void Hop()
    {
        // If rb is null (destroyed), stop executing
        if (rb == null)
        {
            return;
        }
        // Record the direction at the moment of the hop.
        lastDirection = moveDirection;
        Vector2 hopVector = new Vector2(moveDirection * moveSpeed, hopForce);
        rb.AddForce(hopVector, ForceMode2D.Impulse);

        //Change the hop interval to a random value between 2 and 4.
        hopInterval = Random.Range(1.5f, 3.5f);
    }

    // Randomly choose a new horizontal direction, forcing it inward if at boundaries.
    void ChooseNewDirection()
    {
        // If at or beyond the left boundary, force right.
        if (Body.position.x <= leftWorld.x)
        {
            moveDirection = 1f;
        }
        // If at or beyond the right boundary, force left.
        else if (Body.position.x >= rightWorld.x)
        {
            moveDirection = -1f;
        }
        else
        {
            // Otherwise, choose randomly between left (-1) and right (1).
            moveDirection = Random.Range(0, 2) == 0 ? -1f : 1f;
        }
    }

    // In LateUpdate, update the sprite's flip orientation only while airborne.
    void LateUpdate()
    {
        // If Body is null (destroyed), stop executing
        if (Body == null)
        {
            return;
        }

        SpriteRenderer sr = Body.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            // Only update orientation while airborne.
            if (Mathf.Abs(rb.linearVelocity.y) > 0.1f)
            {
                // If last hop direction is left (positive), flip sprite to face left.
                sr.flipX = (lastDirection > 0);
            }
        }
    }


}
