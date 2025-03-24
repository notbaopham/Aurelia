using UnityEngine;
using UnityEngine.UI;
public class UiManager : MonoBehaviour
{
    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Image[] maxHearts;
    [SerializeField] private Player player;

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
}
