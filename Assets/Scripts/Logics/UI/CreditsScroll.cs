using System.Collections;
using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private RectTransform creditsText; // Assign the UI Text or Panel containing the credits
    [SerializeField] private float moveDistance = 7450f; // How far the text moves
    [SerializeField] private float startDelay = 2f; // Delay before starting the scroll
    [SerializeField] private float moveDuration = 10f; // Time to reach the top

    private Vector2 startPosition;
    private Vector2 targetPosition;

    void Start()
    {
        startPosition = creditsText.anchoredPosition;
        targetPosition = startPosition + new Vector2(0, moveDistance); // Move upwards
        StartCoroutine(ScrollCredits());
    }

    private IEnumerator ScrollCredits()
    {
        // Wait for the specified delay before starting the scroll
        yield return new WaitForSeconds(startDelay);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            creditsText.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            yield return null;
        }

        creditsText.anchoredPosition = targetPosition; // Ensure exact position
    }
}
