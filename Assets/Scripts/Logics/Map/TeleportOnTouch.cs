using UnityEngine;

public class TeleportOnTouch : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget; // The object B's transform (where the player will teleport)

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that touched is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched the portal!");

            // Teleport the player to the teleport target
            if (teleportTarget != null)
            {
                other.transform.position = teleportTarget.position;
                Debug.Log("Player teleported to " + teleportTarget.position);
            }
        }
    }
}
