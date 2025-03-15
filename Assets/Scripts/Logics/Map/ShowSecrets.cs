using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeOnPlayerTouch : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;    // Tilemap Object B to fade
    [SerializeField] private float fadeDuration = 2f; // Time it takes for Object B to fade away
    private Color startColor; // Starting color of the Tilemap

    void Start()
    {
        // Make sure Tilemap B is assigned
        if (tilemap != null)
        {
            startColor = tilemap.color; // Store the initial color of the Tilemap
        }
        else
        {
            Debug.LogError("Tilemap is not assigned!");
        }
    }

    // Called when any collider enters the trigger zone (2D)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Debugging the collision
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        // Check if the collider that entered has the "Player" tag
        if (other.CompareTag("Player"))
        {

            // Start fading out the Tilemap B
            if (tilemap != null)
            {
                StartCoroutine(FadeOutTilemapB());
            }
        }
    }

    private IEnumerator FadeOutTilemapB()
    {
        float timeElapsed = 0f;

        // The target color is fully transparent (alpha = 0)
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (timeElapsed < fadeDuration)
        {
            // Lerp the color of the Tilemap to fade out
            tilemap.color = Color.Lerp(startColor, targetColor, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final color is fully transparent
        tilemap.color = targetColor;

        // Optionally, you can disable the Tilemap after it fades out completely
        tilemap.gameObject.SetActive(false);
    }
}
