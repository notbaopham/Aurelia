using System.Collections;
using UnityEngine;

public class WindFollow : MonoBehaviour
{
    [SerializeField] private ParticleSystem windParticleLeft;
    [SerializeField] private ParticleSystem windParticleRight;
    [SerializeField] private Player player;
    [SerializeField] private float particleOffset = 20.0f; // Distance from player
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


    void Update()
    {
        // Make the particle systems follow the player’s position
        windParticleLeft.transform.position = player.transform.position + Vector3.right * particleOffset;
        windParticleRight.transform.position = player.transform.position - Vector3.right * particleOffset;
    }
}
