using UnityEditor;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
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
