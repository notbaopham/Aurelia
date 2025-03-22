using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster") {
            player.Hurt();
        }
    }
}
