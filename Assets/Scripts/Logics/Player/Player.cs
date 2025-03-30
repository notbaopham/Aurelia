using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{

    public static Player Instance;
    // Managers - top objects to refer to
    [SerializeField] private InputManager inputManager;
    private PlayerAudio playerAudio;

    // Movement values
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;
    private bool isMovementKeyOn;
    [SerializeField] private float gravityScale = 2f;
    private float releaseTimer = 20f;

    // Overlord variables
    [SerializeField] private LayerMask consideredGround;
    private bool isOnGround;
    private Rigidbody2D rb;
    private Vector2 currentlyFacing;

    // Dashing variables
    [SerializeField] private bool isDashUnlocked = false;
    private bool isDashing = false;
    [SerializeField] private float dashSpeed = 20f;
    private float dashTime, lastDashTime;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 5f;

    // Double Jump variables
    [SerializeField] private bool isDoubleJumpUnlocked = false;
    private bool canDoubleJump;

    // Attack variable
    private bool isAttacking;
    private GameObject attackArea = default;
    private float timeAttacking = 0.2f;
    private float attackTimer = 0f;
    private bool inAttackSequence;

    // Attack cooldown variables
    [SerializeField] private float attackCooldown = 0.1f;  
    private float lastAttackTime = 0f; // Time of the last attack
    [SerializeField] private float attackRecovery = 0.7f;
    private float attackRecoveryTimer = 0f;
    private bool isInRecovery;
    private bool isAttackingInAir;

    // Attack area variable
    [SerializeField] GameObject myAttackAreaObject;

    // Animation / Sprite Renderer
    [SerializeField] GameObject spriteObject;

    // After image variables (exclusively for After Images effects when dashing)
    public float distanceBetweenImages;
    private float dashTimeLeft;
    private float lastImageXpos;

    // Player's Health and Hurtbox variables
    [SerializeField] int playerHealth = 5;
    private int playerMaxHealth = 5;
    private GameObject playerHurtbox;
    private float hurtDuration = 0.6f;
    private float hurtKnockBack = 5f;
    private bool isHurting;

    // Player's Death variables
    private bool isDying;
    private float dieDuration;

    private float horizontalInput;
    // Start of the player object
    private void Awake()
    {
        if (FindObjectsByType<Player>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return; 
        }
        DontDestroyOnLoad(gameObject);
        playerMaxHealth = playerHealth;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJump.AddListener(Jump);
        inputManager.OnDash.AddListener(StartDash);
        inputManager.OnAttack.AddListener(Attack);
        rb = GetComponent<Rigidbody2D>();
        currentlyFacing = Vector2.right;
        rb.gravityScale = gravityScale;
        playerAudio = GetComponentInChildren<PlayerAudio>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
        playerHurtbox = transform.GetChild(2).gameObject;
        StartCoroutine(DampenVelocity(0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Math.Abs(rb.linearVelocityX) < 0.01f) {
                releaseTimer = 0f;
                isMovementKeyOn = false;
            } else {
                releaseTimer = 20f;
                isMovementKeyOn = true;
            }

        }
        else
        {
            isMovementKeyOn = false;
            StartCoroutine(CountDownTimer());
        }
        spriteObject.GetComponent<Animator>().SetBool("isMovementKeyOn", isMovementKeyOn);
        spriteObject.GetComponent<Animator>().SetFloat("keyReleasedTimer", releaseTimer);

        // Update the recovery timer
        if (attackRecoveryTimer > 0)
        {
            attackRecoveryTimer -= Time.deltaTime;

            if (attackRecoveryTimer <= 0)
            {
                isInRecovery = false; // Recovery has ended
                if (isAttackingInAir) {
                    isAttackingInAir = !isAttackingInAir;
                }
            }
        }
    }

    public bool keyPressingState() {
        return isMovementKeyOn;
    }

    public float getKeyReleasedTime() {
        return releaseTimer;
    }

    private IEnumerator CountDownTimer()
    {
        if (Math.Abs(rb.linearVelocityX) < 0.01f) {
            releaseTimer = 0f;
        }

        while (releaseTimer > 0f)
        {
            yield return new WaitForSeconds(0.05f); 
            releaseTimer -= 0.05f;
        }

        releaseTimer = 0f; // Ensure it stops at 0
    }

    private IEnumerator DampenVelocity(float dampAmount)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            Vector2 newVelocity = rb.linearVelocity;

            if (!isOnGround && !isMovementKeyOn) {
                if (newVelocity.x > 0)
                    newVelocity.x = Mathf.Max(0, newVelocity.x - dampAmount);
                else if (newVelocity.x < 0)
                    newVelocity.x = Mathf.Min(0, newVelocity.x + dampAmount);
                rb.linearVelocity = newVelocity;
            }
        }
    }

    void FixedUpdate()
    {
        /* First implementation
        Vector2 velocity = rb.linearVelocity;
        
        if (velocity.magnitude > maxSpeed) {
            velocity = velocity.normalized * maxSpeed;
        }
        rb.linearVelocity = new Vector2(velocity.x, rb.linearVelocity.y);
        */

        // Movement speed capper

        if (Math.Abs(rb.linearVelocityX) > maxSpeed) {
            if (rb.linearVelocityX < 0) {
                rb.linearVelocityX = -maxSpeed;
            } else if (rb.linearVelocityX > 0) {
                rb.linearVelocityX = maxSpeed;
            }
        } 

        float turnThreshhold = 0.1f;
        // Rotator
        if (Math.Abs(rb.linearVelocityX) > turnThreshhold && isMovementKeyOn && !isHurting) {
            if (rb.linearVelocityX < 0) {
                currentlyFacing = Vector2.left;
            } else if (rb.linearVelocityX > 0) {
                currentlyFacing = Vector2.right;
            }
        }
    

        // Dashing check
        if (isDashing) {
            DashMovement();
        }

        // Ground collision check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, consideredGround);
        Vector2 backRayPos = new Vector2(transform.position.x - (currentlyFacing.x * 0.35f), transform.position.y);
        Vector2 frontRayPos = new Vector2(transform.position.x - (currentlyFacing.x * -0.35f), transform.position.y);

        // Extra RayCasting, to prevent player dangling
        RaycastHit2D backRay = Physics2D.Raycast(backRayPos, Vector2.down, 0.6f, consideredGround);
        RaycastHit2D frontRay = Physics2D.Raycast(frontRayPos, Vector2.down, 0.6f, consideredGround);
        Debug.DrawLine(backRayPos, transform.position, Color.blue);
        Debug.DrawLine(frontRayPos, transform.position, Color.blue);

        if (hit || backRay || frontRay) {
            isOnGround = true;
            canDoubleJump = true;
        } else {
            isOnGround = false;
        }

        // Updating isJumping variable in Animator
        spriteObject.GetComponent<Animator>().SetBool("isJumping", !isOnGround);
        spriteObject.GetComponent<Animator>().SetBool("isInRecovery", isInRecovery);

        // Attack management
        if (isAttacking) {

            attackTimer += Time.deltaTime;

            if (attackTimer >= timeAttacking) {
                attackTimer = 0f;
                isAttacking = false;
                attackArea.SetActive(isAttacking);
                isInRecovery = true;
            }
        }
        
        // Adjusting the Area to be left and right, as rotation
        if (currentlyFacing == Vector2.left) {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Rotate 180° around the Y-axis
            /*
            myAttackAreaObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            spriteObject.GetComponent<SpriteRenderer>().flipX = true;
            */
        } else if (currentlyFacing == Vector2.right) {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Rotate 180° around the Y-axis
            /*
            myAttackAreaObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            spriteObject.GetComponent<SpriteRenderer>().flipX = false;
            */
        }

        // Updating speed in Animator
        spriteObject.GetComponent<Animator>().SetFloat("xVelocity", Mathf.Abs(rb.linearVelocityX));
        spriteObject.GetComponent<Animator>().SetFloat("yVelocity", rb.linearVelocityY);

        // Updating dash in Animator
        spriteObject.GetComponent<Animator>().SetBool("isDashing", isDashing);
        spriteObject.GetComponent<Animator>().SetBool("isAttacking", isAttacking);

        // Updating attack sequence
        spriteObject.GetComponent<Animator>().SetBool("isInAttackSequence", isAttacking || isInRecovery);
        spriteObject.GetComponent<Animator>().SetBool("isAttackingInAir", isAttackingInAir);

        // Updating hurt sequence
        spriteObject.GetComponent<Animator>().SetBool("isHurting", isHurting);

        // Updating wind sequence
        float modifiedAcceleration = acceleration;
        if (Wind.Instance.windActive && !isDashing)
        {
            Vector2 windDir = Wind.Instance.windDirection == Wind.WindDirection.Left ? Vector2.left : Vector2.right;
            float windForce = Wind.Instance.windForce;

            if (!isMovementKeyOn)
            {
                rb.linearVelocity = new Vector2(windDir.x * windForce, rb.linearVelocity.y);
            }
            else
            {
                float inputDir = horizontalInput;
                if ((inputDir > 0 && windDir == Vector2.right) || (inputDir < 0 && windDir == Vector2.left))
                {
                    modifiedAcceleration += windForce;
                }
                else if ((inputDir > 0 && windDir == Vector2.left) || (inputDir < 0 && windDir == Vector2.right))
                {
                    modifiedAcceleration -= windForce;
                }
            }
        }

        if (!isDashing && !isAttacking && !isHurting)
        {
            rb.AddForce(new Vector2(horizontalInput * modifiedAcceleration, 0f));
        }
    }

    // ---------- Player's Jump, Movement and Dash ----------

    void Jump() {
        // Debug.Log("Jumping");
        // Debug.Log(isOnGround);
        if (isHurting) {
            return;
        }

        if (isOnGround) {
            playerAudio.PlayJumpStart();
            Vector2 jumpDir = Vector2.up;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
        } else if (canDoubleJump) {
            if (!isDoubleJumpUnlocked) {
                return; // Ignore double jump if not unlocked
            }
            playerAudio.PlayDoubleJumpStart();
            Vector2 jumpDir = Vector2.up;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
        
    }

    private void MovePlayer(Vector2 directionInput) {
        if (isInRecovery || isHurting)
        {
            return; // Ignore movement during recovery
        }
        if (!isAttacking)
            rb.AddForce(directionInput * acceleration);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            playerAudio.PlayJumpLand();
        }
    }

    void StartDash()
    {
        if (!isDashUnlocked)
        {
            return; // Ignore dashing if not unlocked
        }
        if (Time.time - lastDashTime < dashCooldown || isHurting)
        {
            return; // Prevent dashing if cooldown has not expired
        }

        // Initiate dash and record the time
        isDashing = true;
        dashTime = 0f; // Reset dash duration
        lastDashTime = Time.time; // Set cooldown timer

        // Disable gravity during dash
        rb.gravityScale = 0f; // Disable gravity

        // Plays Audio
        playerAudio.PlayDash();

        lastImageXpos = transform.position.x;

    }

    void DashMovement()
    {
        // Dash for the specified duration
        dashTime += Time.deltaTime;

        if (dashTime < dashDuration)
        {
            rb.linearVelocity = currentlyFacing * dashSpeed; // Move in dash direction

            if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages) {
                PlayerAfterImagesPool.Instance.GetFromPool();
                lastImageXpos = transform.position.x;
            }
        }
        else
        {
            isDashing = false; // End the dash after the duration
            // rb.linearVelocity = Vector2.zero; // Stop movement after dash ends

            // Restore gravity after dash ends
            rb.gravityScale = gravityScale; // Restore original gravity scale
        }
    }

    // ---------- Player's Attacks and Sequencer ----------

    void Attack() {

        // Break if in cooldown, or currently dashing
        if ((Time.time - lastAttackTime < (attackCooldown + attackRecovery)) || isDashing || isHurting)
        {
            // Attack is on cooldown
            return;
        }

        // Attack logics
        isInRecovery = false;

        isAttacking = true;

        if (!isOnGround) {
            isAttackingInAir = true;
            playerAudio.PlayAttack("air");
            StartCoroutine(DelayBeforeAttack(0.1f));
            attackArea.SetActive(isAttacking);
        } else {
            playerAudio.PlayAttack("ground");
            StartCoroutine(DelayBeforeAttack(0.2f));
            attackArea.SetActive(isAttacking);
        }

        lastAttackTime = Time.time; // Record time

        attackTimer = 0f; // Reset time after

        attackRecoveryTimer = attackRecovery;
    }

    private IEnumerator DelayBeforeAttack(float time) {
        yield return new WaitForSeconds(time);
    }

    // ---------- Getters, Setters and Status Checkers ----------

    public bool DashCheck() {
        return !(Time.time - lastDashTime < dashCooldown);
    }

    public bool DoubleJumpCheck() {
        return canDoubleJump;
    }

    public int GetHealth() {
        return playerHealth;
    }
    public int GetMaxHealth() {
        return playerMaxHealth;
    }   

    // ---------- Player's Heath, TakeDamage and Death ----------

    public void TakeDamage(int damageTaken, bool existsKnockback) {
        if (!isHurting)
        {
            StartCoroutine(HurtSequence(existsKnockback));

            if (playerHealth - damageTaken <= 0) 
            {
                playerHealth = 0;
                StartCoroutine(DieWithDelay());
            } 
            else 
            {
                playerHealth -= damageTaken;    
            }
        }
    }
    public void AddBonusHealth(int healthBonus) {
        playerMaxHealth += healthBonus;
    }
    public void Heal(int heal) {
        playerAudio.PlayHeal();
        playerHealth += heal;
        if (playerHealth > playerMaxHealth) {
            playerHealth = playerMaxHealth;
        }
    }
    public void HealFull() {
        playerHealth = playerMaxHealth;
    }

    public void Hurt(int damage, bool existsKnockback) {
        if (isDashing) {
            return;
        }
        playerAudio.PlayHurt();
        TakeDamage(damage, existsKnockback);
    }

    private void Death() {
        Destroy(gameObject);
    }

    private IEnumerator HurtSequence(bool existsKnockback) {

        isHurting = true;

        if (existsKnockback) {
            // Kills velocity, and knockback by 45* facing
            rb.linearVelocity = Vector2.zero;
            float knockbackSide = (currentlyFacing == Vector2.right) ? -1 : 1;
            Vector2 knockbackDirection = new Vector2(knockbackSide, 1).normalized;
            rb.AddForce(knockbackDirection * hurtKnockBack, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(hurtDuration);

        isHurting = false;
        if (isOnGround) {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private IEnumerator DieWithDelay() {
        yield return new WaitForSeconds(hurtDuration);
        Death();
    }
    public bool IsDoubleJumpUnlocked() {
        return isDoubleJumpUnlocked;
    }
    public bool IsDashUnlocked() {
        return isDashUnlocked;
    }
    public void UnlockDoubleJump() {
        isDoubleJumpUnlocked = true;
    }
    public void UnlockDash() {
        isDashUnlocked = true;
    }
    

    public bool getGroundedState() {
        return isOnGround;
    }

    public bool getDashingState() {
        return isDashing;
    }

    public bool getAttackingState() {
        return isAttacking || isInRecovery;
    }
}
