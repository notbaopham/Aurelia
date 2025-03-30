using System.Collections;
using UnityEngine;

public class SecretItem : MonoBehaviour
{
    [SerializeField] private Player player; // The player object
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(AssignPlayerAfterDelay());
    }
    private IEnumerator AssignPlayerAfterDelay()
    {
        yield return new WaitForSeconds(1);

        // Find all active Player objects
        Player[] players = FindObjectsByType<Player>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        if (players.Length > 0)
        {
            player = players[0];
            Debug.Log("Player assigned!");
        }
        else
        {
            Debug.LogWarning("No Player instance found!");
        }
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
