using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, dashSound, healPotionPop, healSoothingVoice, attackSound, runSound;
    private Player player;

    public void PlayWalkSound() => audioSource.PlayOneShot(runSound);

    public float runSoundInterval = 2f;  // Time interval between each sound play
    private float timeSinceLastRunSound = 0f;  // Timer to track intervals

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 1.5f && player.getGroundedState() && !player.getDashingState() && !player.getAttackingState())
        {
            timeSinceLastRunSound += Time.deltaTime;

            if (timeSinceLastRunSound >= runSoundInterval)
            {
                audioSource.PlayOneShot(runSound);
                timeSinceLastRunSound = 0f;
            }
        }
    }
}
