using UnityEngine;

public class SecretItem : MonoBehaviour
{
    [SerializeField] private Player player; // The player object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Replace "Player" with your player's tag
        {
            Destroy(gameObject); // Destroys this object (Object A)
            player.AddBonusHealth(1); // Bonus the player's health
            player.Heal(1); // Heals the player
        }
    }
}
