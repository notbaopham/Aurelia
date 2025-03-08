using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void TakeDamage(float damage) {

        if (health - damage <= 0) {
            Death();
        } else {
            health -= damage;
        }

    }

    private void Death() {
        // Anh Hoc co the implement effects right here
        /*--------------------------------------------


        ----------------------------------------------*/
        // Destroy the object at the end
        Destroy(transform.gameObject);
    }
}
