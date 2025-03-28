using Mono.Cecil;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound, dashSound, healPotionPop, healSoothingVoice, attackSound, runSound;
    private AudioClip[] runSounds, jumpStartSounds, jumpLandSounds;
    private Player player;

    public void PlayWalkSound() => audioSource.PlayOneShot(runSound);

    public float runSoundInterval = 2f;  // Time interval between each sound play
    private float timeSinceLastRunSound = 0f;  // Timer to track intervals

    void Awake()
    {
        runSounds = Resources.LoadAll<AudioClip>("Audio/Run/Grass");
        jumpStartSounds = Resources.LoadAll<AudioClip>("Audio/Jump/JumpOff");
        jumpLandSounds = Resources.LoadAll<AudioClip>("Audio/Jump/Landing");
    }
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
                if (runSounds.Length > 0)
                {
                    int randomIndex = Random.Range(0, runSounds.Length);
                    audioSource.PlayOneShot(runSounds[randomIndex]);
                }
                timeSinceLastRunSound = 0f;
            }
        }
    }

    public void PlayJumpStart() {
        int randomIndex = Random.Range(0, jumpStartSounds.Length);
        audioSource.PlayOneShot(jumpStartSounds[randomIndex]);
    }

    public void PlayJumpLand() {
        int randomIndex = Random.Range(0, jumpLandSounds.Length);
        audioSource.PlayOneShot(jumpLandSounds[randomIndex]);
    }

    public void PlayDoubleJumpStart() {
        
    }

    public void PlayAttack(string attackType) {
        if (attackType == "air") {

        }
        if (attackType == "ground") {
            
        }
    }
}
