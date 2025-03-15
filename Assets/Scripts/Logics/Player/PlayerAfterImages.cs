using UnityEngine;

public class PlayerAfterImages : MonoBehaviour
{
    private float activeTime = 0.1f;
    private float timeActivated;
    private float alpha;
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;
    private Transform player;

    private SpriteRenderer SR;
    private SpriteRenderer playerSR;

    private Color color;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetChild(1).GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        SR.sprite = playerSR.sprite;
        // transform.position = player.position;
        transform.position = new Vector2(player.position.x, player.position.y - 0.5f);
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void FixedUpdate()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        if (Time.time >= (timeActivated + activeTime)) {
            PlayerAfterImagesPool.Instance.AddToPool(gameObject);
        }
    }
}
