using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image doubleJumpBorder;
    
    [SerializeField] private Image dashImage;
    [SerializeField] private Image dashBorder;

    [SerializeField] private Image[] hearts;
    [SerializeField] private Image[] maxHearts;
    private int previousHealth;
    private int previousMaxHealth;
    [SerializeField] private Player player;

    [SerializeField] ChangeOfScene changeOfScene; // Reference to the ChangeOfScene script
    [SerializeField] CanvasGroup blackScreen; // Reference to the black screen
    [SerializeField] CanvasGroup endScreen; // Reference to the end screen

    [SerializeField] float fadeDuration = 1f;
    private bool isSceneChanging = false;

    // Variables for wind text warning and flickering effect
    [SerializeField] private TMP_Text warningText;
    [SerializeField] private TMP_Text durationText;
    private Coroutine flickerRoutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previousHealth = player.GetHealth();
        previousMaxHealth = player.GetMaxHealth();
        UpdateHealthUI(player.GetHealth());
        UpdateMaxHealthUI(player.GetMaxHealth());
       
        if(!player.IsDoubleJumpUnlocked())
        {
            doubleJumpImage.enabled = false;
            doubleJumpBorder.enabled = false;
        }

         if(!player.IsDashUnlocked())
        {
            dashImage.enabled = false;
            dashBorder.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!player.IsDoubleJumpUnlocked())
        {
            doubleJumpImage.enabled = false;
            doubleJumpBorder.enabled = false;
        }
        else
        {
            doubleJumpImage.enabled = true;
            doubleJumpBorder.enabled = true;
        }

        if(!player.IsDashUnlocked())
        {
            dashImage.enabled = false;
            dashBorder.enabled = false;
        }
        else
        {
            dashImage.enabled = true;
            dashBorder.enabled = true;
        }
        UpdateSkillImage(player.DoubleJumpCheck(), doubleJumpImage);
        UpdateSkillImage(player.DashCheck(), dashImage);
        // if (Player.Instance != null)
        // {
        //     Color imageColor = dashImage.color;
        //     imageColor.a = Player.Instance.canDash ? 1f : 0.3f;
        //     dashImage.color = imageColor;
            
        // }
        if (previousHealth != player.GetHealth())
        {
            UpdateHealthUI(player.GetHealth());
            previousHealth = player.GetHealth();
        }

        if (previousMaxHealth != player.GetMaxHealth())
        {
            UpdateMaxHealthUI(player.GetMaxHealth());
            previousMaxHealth = player.GetMaxHealth();
        }

        if (player.GetHealth() <= 0 && !isSceneChanging)
        {
            isSceneChanging = true;
            StartCoroutine(GameOver());
        }
    }  
    
    void UpdateSkillImage(bool skill, Image image)
    {
        if (Player.Instance != null)
        {
            Color imageColor = image.color;
            imageColor.a = skill ? 1f : 0.3f;
            image.color = imageColor;
            
        }
    }
    public void UpdateHealthUI(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                hearts[i].enabled = true; // Show the heart
            }
            else
            {
                hearts[i].enabled = false; // Hide the heart
            }
        }
    }
    public void UpdateMaxHealthUI(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < health)
            {
                maxHearts[i].enabled = true; // Show the heart
            }
            else
            {
                maxHearts[i].enabled = false; // Hide the heart
            }
        }
    }

    //-------------------------Canvas Fading--------------------------------------------
    private IEnumerator GameOver()
    {

        changeOfScene.FadeIn(blackScreen);
        
        yield return new WaitForSeconds(2f);

        changeOfScene.FadeIn(endScreen);
        yield return new WaitForSeconds(4f);
        changeOfScene.FadeOut(endScreen);
        yield return new WaitForSeconds(fadeDuration + 0.5f);
        Destroy(GameObject.Find("Player 1"));
        Destroy(GameObject.Find("Player After Image Pool"));
        Destroy(GameObject.Find("Game Manager"));
        Destroy(GameObject.Find("Settings Canvas"));
        Destroy(GameObject.Find("Player UI Canvas"));
        SwitchScene("MainMenu");
    }
    //----------------------------Scene Changer---------------------------------------------
    private void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    //-------------------------Wind Warning Text--------------------------------------------
    public void ShowWarning(string direction, float time)
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(WarningCountdown(direction, time));

    }

    private IEnumerator WarningCountdown(string direction, float duration)
    {
        int timeLeft = Mathf.CeilToInt(duration);
        string arrowSymbols = direction.ToLower() == "left" ? "<<<" : ">>>";
        while (timeLeft > 0)
        {
            string message = $"Wind in {timeLeft}s {arrowSymbols}";
            warningText.text = message;

            // Flicker on
            warningText.enabled = true;
            yield return new WaitForSeconds(0.5f);

            // Flicker off
            warningText.enabled = false;
            yield return new WaitForSeconds(0.5f);

            timeLeft--;
        }

        warningText.enabled = false;
    }

    //-------------------------Wind Duration Text--------------------------------------------
    public void ShowWindDuration(float duration)
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        StartCoroutine(WindDurationCountdown(duration));
    }

    private IEnumerator WindDurationCountdown(float duration)
    {
        float timeLeft = duration;

        durationText.enabled = true;
        durationText.text = $"Wind Duration: {Mathf.CeilToInt(timeLeft)}s";
        yield return new WaitForSeconds(2f);

        durationText.enabled = false;
        durationText.text = "";
    }

}

