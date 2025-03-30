using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private AudioClip[] runSounds, jumpStartSounds, jumpLandSounds, doubleJumpSounds, dashSounds, attackVoices, healPotionPops, healSoothingVoices, hurtSounds, hurtVoices;
    private string currentMapName;
    private Player player;
    private float runSoundInterval = 0.4f;  // Time interval between each sound play
    private float timeSinceLastRunSound = 0f;  // Timer to track intervals

    void Awake()
    {
        jumpStartSounds = Resources.LoadAll<AudioClip>("Audio/Jump/JumpOff");
        jumpLandSounds = Resources.LoadAll<AudioClip>("Audio/Jump/Landing");
        doubleJumpSounds = Resources.LoadAll<AudioClip>("Audio/Jump/DoubleJump");
        dashSounds = Resources.LoadAll<AudioClip>("Audio/Dash");
        attackVoices = Resources.LoadAll<AudioClip>("Audio/Attack/VoiceAttack");
        healPotionPops = Resources.LoadAll<AudioClip>("Audio/Heal/PotionPop");
        healSoothingVoices = Resources.LoadAll<AudioClip>("Audio/Heal/VoiceHeal");
        hurtSounds = Resources.LoadAll<AudioClip>("Audio/Hurt/HurtSound");
        hurtVoices = Resources.LoadAll<AudioClip>("Audio/Hurt/HurtVoices");

    }

    void Start()
    {   
        string sceneName = SceneManager.GetActiveScene().name;
        player = GetComponentInParent<Player>();
        audioSource = GetComponentInChildren<AudioSource>();

        // Switching based on the current Map
        switch (sceneName) {
            case "Map1Scene":
                runSounds = Resources.LoadAll<AudioClip>("Audio/Run/Grass");
                break;
            case "Map2Scene":
                runSounds = Resources.LoadAll<AudioClip>("Audio/Run/Snow");
                break;
            case "Map3Scene":
                runSounds = Resources.LoadAll<AudioClip>("Audio/Run/Desert");
                break;
            default:
                runSounds = Resources.LoadAll<AudioClip>("Audio/Run/Grass");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.GetComponent<Rigidbody2D>().linearVelocity.x) > 1.5f && player.getGroundedState() && !player.getDashingState() && !player.getAttackingState() && (player.getKeyReleasedTime() > 0))
        {
            timeSinceLastRunSound += Time.deltaTime;

            if (timeSinceLastRunSound >= runSoundInterval)
            {
                if (runSounds.Length > 0)
                {
                    PlaySound(runSounds, 1.0f);
                }
                timeSinceLastRunSound = 0f;
            }
        }
    }

    public void PlayJumpStart() {
        PlaySound(jumpStartSounds, 1.0f);
    }

    public void PlayJumpLand() {
        PlaySound(jumpLandSounds, 1.0f);
    }

    public void PlayDoubleJumpStart() {
        PlaySound(doubleJumpSounds, 1.0f);
    }

    public void PlayDash() {
        PlaySound(dashSounds, 1.0f);
    }

    public void PlayAttack(string attackType) {
        if (attackType == "air") {
            StartCoroutine(AttackAirSound());
        }
        if (attackType == "ground") {
            StartCoroutine(AttackGroundSound());
        }
    }

    public void PlayHeal() {
        StartCoroutine(PotionPopThenSoothe());
    }

    public void PlayHurt() {
        audioSource.pitch = Random.Range(0.7f, 0.9f);
        PlaySound(hurtVoices, 1.0f);
        PlaySound(hurtSounds, 1.0f);
        audioSource.pitch = 1.0f;
    }

    private void PlaySound(AudioClip[] audioArray, float volume) {
        int randomIndex = Random.Range(0, audioArray.Length);
        audioSource.PlayOneShot(audioArray[randomIndex]);
    }

    private IEnumerator AttackGroundSound() {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        PlaySound(attackVoices, 1.0f);
        PlaySound(doubleJumpSounds, 1.0f);
        yield return new WaitForSeconds(0.35f);
        PlaySound(doubleJumpSounds, 1.0f);
    }

    private IEnumerator AttackAirSound() {
        yield return new WaitForSeconds(0.25f);
        PlaySound(doubleJumpSounds, 1.0f);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        PlaySound(attackVoices, 1.0f);
    }

    private IEnumerator PotionPopThenSoothe() {
        PlaySound(healPotionPops, 1.0f);
        yield return new WaitForSeconds(0.5f);
        PlaySound(healSoothingVoices, 1.0f);
    }
}
