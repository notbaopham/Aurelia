using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Destroy and hopefully recreates a new character
    void TakeDamage(int damageTaken) {

        if (playerHealth - damageTaken <= 0) {

            Death();

        } else {

            playerHealth -= damageTaken;

        }
        
    }

    void Death() {

        Destroy(this);

    }
}
