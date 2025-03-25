using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeOfScene : MonoBehaviour
{
    [SerializeField] private float disableDuration = 2f; // Time to disable input
    [SerializeField] private string sceneName; // Name of the scene to change to
    [SerializeField] private InputManager inputManager; // Reference to the InputManager
    [SerializeField] CanvasGroup blackScreen; // Reference to the black screen
    [SerializeField] CanvasGroup endScreen; // Reference to the end screen

    [SerializeField] float fadeDuration = 1f;
    private Player player;

    private void Start()
    {
        DontDestroyOnLoad(this);
        player = FindFirstObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player touched trigger — disabling movement.");
            StartCoroutine(ChangingScene(disableDuration));
        }
    }

    private IEnumerator ChangingScene(float duration)
    {
        inputManager.isMovementDisabled = true;
        Debug.Log("Movement Disabled");
        FadeIn(blackScreen);
        
        yield return new WaitForSeconds(duration);
        
        inputManager.isMovementDisabled = false;
        Debug.Log("Movement Enabled");
        FadeIn(endScreen);
        yield return new WaitForSeconds(4f);
        FadeOut(endScreen);
        yield return new WaitForSeconds(fadeDuration + 0.5f);
        SwitchScene(sceneName);
    }
    public void FadeIn(CanvasGroup canvasGroup)
    {
        StartCoroutine(FadeCanvas(canvasGroup, 0f, 1f, fadeDuration));
    }

    public void FadeOut(CanvasGroup canvasGroup)
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
    private void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        switch (sceneName) {
            case "MainMenu":
                break;
            case "Map1Scene":
                player.unlockDoubleJump();                
                break;
            case "Map2Scene":
                player.unlockDoubleJump();
                player.unlockDash();
                break;
            default:
                return;
        }
    }
}
