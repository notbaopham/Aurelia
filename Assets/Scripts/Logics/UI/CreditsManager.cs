using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class CreditsManager : MonoBehaviour
{
    // [SerializeField] private GameObject creditsText; 
    // [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private Image BlackPanel;
    [SerializeField] private float fadeDuration = 1.5f; // Time to fade
    [SerializeField] private float delayBeforeAppear = 10f; // Time to stay black
    [SerializeField] private CanvasGroup mainScreen; // Reference to the end screen
    [SerializeField] private Image whitePanel; // Reference to the end screen
    [SerializeField] private CanvasGroup creditsText; // Reference to the end screen

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(AppearAfterDelay());
    }
    private IEnumerator AppearAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeAppear - 3.5f); // Wait for 1 second
        StartCoroutine(StartWhiteScreen());
        yield return new WaitForSeconds(3.5f); // Wait for 2 seconds

        Debug.Log("FadeIn Called");
        StartCoroutine(StartFadeIn()); // Start the fade-in effect

        // Destroy(gameObject); // Destroy this object (CreditsManager)
    }
    private IEnumerator StartWhiteScreen()
    {
        float elapsedTime = 0f;
        Color color = whitePanel.color;
        
        while (elapsedTime < 3.5f)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, elapsedTime / 3.5f);
            whitePanel.color = color;
            yield return null;
        }
    }
    private IEnumerator StartFadeIn()
    {
        Debug.Log("FadeIn started");
        // yield return new WaitForSeconds(delayBeforeFade);
        float elapsedTime = 0f;
        Color color = BlackPanel.color;
        
        FadeIn(mainScreen, fadeDuration - 1); // Fade out the main screen
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            BlackPanel.color = color;
            yield return null;
        }

        BlackPanel.gameObject.SetActive(false); // Hide panel after fade
    }
    // Update is called once per frame
    private void FadeIn(CanvasGroup canvasGroup, float fadeDuration)
    {
        StartCoroutine(FadeCanvas(canvasGroup, 0f, 1f, fadeDuration));
    }

    private void FadeOut(CanvasGroup canvasGroup, float fadeDuration)
    {
        StartCoroutine(FadeCanvas(canvasGroup, 1f, 0f, fadeDuration));
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        canvasGroup.gameObject.SetActive(true);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }
        canvasGroup.alpha = endAlpha;

        // Optionally disable the Canvas after fading out
        if (endAlpha == 0f)
        {
            canvasGroup.gameObject.SetActive(false);
        }
    }
}
