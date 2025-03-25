using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image[] maxHearts;
    [SerializeField] private Player player;

    [SerializeField] ChangeOfScene changeOfScene; // Reference to the ChangeOfScene script
    [SerializeField] CanvasGroup blackScreen; // Reference to the black screen
    [SerializeField] CanvasGroup endScreen; // Reference to the end screen

    [SerializeField] float fadeDuration = 0.5f;
    private bool isSceneChanging = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillImage(Player.Instance.DoubleJumpCheck(), doubleJumpImage);
        UpdateSkillImage(Player.Instance.DashCheck(), dashImage);
        // if (Player.Instance != null)
        // {
        //     Color imageColor = dashImage.color;
        //     imageColor.a = Player.Instance.canDash ? 1f : 0.3f;
        //     dashImage.color = imageColor;
            
        // }
        UpdateHealthUI(player.GetHealth());
        UpdateMaxHealthUI(player.GetMaxHealth());
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
        SwitchScene("MainMenu");
    }
    //----------------------------Scene Changer---------------------------------------------
    private void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

