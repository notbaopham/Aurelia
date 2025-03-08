using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] float damage = 3f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("I just hit something");
        if (collider.GetComponent<Health>() != null) {
            Health health = collider.GetComponent<Health>();
            health.TakeDamage(damage);
        }
    }
}
