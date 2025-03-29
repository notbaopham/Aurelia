using UnityEngine;

public class WindFollow : MonoBehaviour
{
    [SerializeField] private ParticleSystem windParticleLeft;
    [SerializeField] private ParticleSystem windParticleRight;
    [SerializeField] private Transform player;
    [SerializeField] private float particleOffset = 20.0f; // Distance from player
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame


    void Update()
    {
        // Make the particle systems follow the player’s position
        windParticleLeft.transform.position = player.position + Vector3.right * particleOffset;
        windParticleRight.transform.position = player.position - Vector3.right * particleOffset;
    }
}
