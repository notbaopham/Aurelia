using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 10f;
    [SerializeField] private float knockbackForce = 0.2f; // Adjust as needed
    [SerializeField] private float invulnerabilityDuration = 1f; // Time the slime is invulnerable
    [SerializeField] private int blinkCount = 3; // Number of times it blinks
    [SerializeField] private float deathBlinkDuration = 0.5f; // Time before disappearing

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage, Vector2 knockbackDirection)
    {
        // Apply knockback force if Rigidbody2D exists
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // Reset velocity before applying force
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

        // Reduce health
        health -= damage;

        if (health > 0)
        {
            StartCoroutine(BlinkEffect());
        }
        else
        {
            StartCoroutine(DeathEffect());
        }
    }

    private IEnumerator BlinkEffect()
    {
        rb.bodyType = RigidbodyType2D.Kinematic; // Prevent falling
        col.enabled = false; // Disable collider for invulnerability

        for (int i = 0; i < blinkCount; i++)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f); // Make invisible
            yield return new WaitForSeconds(invulnerabilityDuration / (blinkCount * 2));
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f); // Make visible
            yield return new WaitForSeconds(invulnerabilityDuration / (blinkCount * 2));
        }

        col.enabled = true; // Enable collider after invulnerability
        rb.bodyType = RigidbodyType2D.Dynamic; // Restore physics interaction
    }

    private IEnumerator DeathEffect()
    {
        rb.bodyType = RigidbodyType2D.Kinematic; // Disable movement
        col.enabled = false; // Disable collision

        float blinkTime = 0f;
        while (blinkTime < deathBlinkDuration)
        {
            sr.color = Color.red; // Turn red
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white; // Restore original color
            yield return new WaitForSeconds(0.1f);
            blinkTime += 0.2f;
        }

        Destroy(gameObject);
    }
}
