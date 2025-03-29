using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
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
            Debug.Log("Player from Scene 1 assigned!");
        }
        else
        {
            Debug.LogWarning("No Player instance found!");
        }
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
