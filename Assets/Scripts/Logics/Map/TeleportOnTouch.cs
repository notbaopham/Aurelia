using UnityEngine;

public class TeleportOnTouch : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // The object B's transform (where the player will teleport)
    [SerializeField] private Player player; // The player object

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the border");

            // Teleport the player to the teleport target
            if (teleportTarget != null)
            {
                player.Hurt(1, false); // Hurt the player
                other.transform.position = teleportTarget.position;
                Debug.Log("Player teleported to " + teleportTarget.position);
            }
        }
    }
}
