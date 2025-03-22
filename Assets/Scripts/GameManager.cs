using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image doubleJumpImage;
    [SerializeField] private Image dashImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSkillImage(Player.Instance.canDoubleJump, doubleJumpImage);
        UpdateSkillImage(Player.Instance.canDash, dashImage);
        // if (Player.Instance != null)
        // {
        //     Color imageColor = dashImage.color;
        //     imageColor.a = Player.Instance.canDash ? 1f : 0.3f;
        //     dashImage.color = imageColor;
            
        // }
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
}
