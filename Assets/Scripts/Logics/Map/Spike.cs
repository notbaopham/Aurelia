using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Player player; // The player object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the spike");
            player.Hurt(1, true); // Hurt the player
        }
    }
}
