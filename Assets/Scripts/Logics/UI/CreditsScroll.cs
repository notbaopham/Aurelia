using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour
{
    public Transform objectA; // First line of the credits
    public Transform objectB; // Last line of the credits
    [SerializeField] private Transform creditsScreen; // Reference to the credits screen
    public float duration = 5f; // Time to complete movement

    private Vector3 objectAStartPos;
    private Vector3 objectBStartPos;
    private Vector3 moveDistance; // Distance to move up

    void Start()
    {
        // Store initial positions
        objectAStartPos = objectA.position;
        objectBStartPos = objectB.position;

        // Calculate how much to move both objects up
        moveDistance = objectBStartPos - objectAStartPos;

        // Start moving both objects
        StartCoroutine(MoveCredits());
    }

    private IEnumerator MoveCredits()
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            // Move both objects up by the same amount
            // objectA.position = Vector3.Lerp(objectAStartPos, objectAStartPos + moveDistance, t);
            // objectB.position = Vector3.Lerp(objectBStartPos, objectAStartPos, t); // B moves to A's start
            creditsScreen.position = Vector3.Lerp(creditsScreen.position, creditsScreen.position + moveDistance, t); // Move the credits screen

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure they reach the final position exactly
        objectA.position = objectAStartPos + moveDistance;
        objectB.position = objectAStartPos;
    }
}
