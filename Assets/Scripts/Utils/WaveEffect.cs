using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    public float amplitude = 0.5f; // Height of the wave
    public float frequency = 1f; // Speed of the wave
    public float waveOffset = 0.5f; // Phase offset for each object

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        // Offset the wave based on the object's X position (or other order factor)
        float yOffset = Mathf.Sin(Time.time * frequency + transform.position.x * waveOffset) * amplitude;
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}
