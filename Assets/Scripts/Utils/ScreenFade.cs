using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.5f; // Time to fade
    [SerializeField] private float delayBeforeFade = 3f; // Time to stay black
    private Image panelImage;

    void Start()
    {
        panelImage = GetComponent<Image>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delayBeforeFade);
        float elapsedTime = 0f;
        Color color = panelImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            panelImage.color = color;
            yield return null;
        }

        panelImage.gameObject.SetActive(false); // Hide panel after fade
    }
}
