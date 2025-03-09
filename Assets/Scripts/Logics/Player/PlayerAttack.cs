using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damage = 3f;
    [SerializeField] private float knockbackForce = 5f; // Adjustable in the Inspector

    private void OnTriggerEnter2D(Collider2D collider)
    {

        // Check if the object has a Health component
        Health health = collider.GetComponent<Health>();
        if (health != null)
        {
            // Calculate knockback direction (push the target away from the player)
            Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;

            // Apply damage and knockback effect
            health.TakeDamage(damage, knockbackDirection * knockbackForce);
        }
    }
}
