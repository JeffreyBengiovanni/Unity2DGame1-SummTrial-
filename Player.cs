using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : Mover
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer holdingSpriteRenderer;
    private Transform weaponSprite;
    public bool canRoll, rolledWall, healthLoaded;
    private Vector3 moveDir, scalar, rollDir;
    private const float MOVE_SPEED = 1.5f;
    float rollSpeed;
    private State state;
    private static Player playerInstance;
    private float roundModifier = 1f;
    private Vector3 velocity;
    private Animator spriteAnim;
    public float rollCooldown = 1.8f;
    public float manaAmount = 20f;
    public float maxMana = 20f;
    public bool isBerserk = false;
    public bool isInvincible = false;
    public bool canRegen = false;
    public bool onScreen;

    public SpriteRenderer s0, s1, s2, s3;

    // Health Bar
    public Transform healthBar;
    public Transform healthDamageBar;
    private float damageHealthTimer;
    private const float MAX_DAMAGE_HEALTH_TIMER = .3f;
    public Text healthBarText;

    // Mana Bar
    public Transform manaBar;


    public bool moving = false;

    private enum State
    {
        Normal,
        Rolling,
    }
    
    protected void Awake()
    {
        DontDestroyOnLoad(this);

        if (playerInstance == null)
        {
            playerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }



    //playerHit = transform.Find("Sounds").Find("PlayerHit").GetComponent<AudioSource>();
        spriteRenderer = transform.Find("PlayerSprite").GetComponent<SpriteRenderer>();
        spriteAnim = transform.Find("PlayerSprite").GetComponent<Animator>();
        holdingSpriteRenderer = transform.Find("Aim").Find("WeaponSprite").GetComponent<SpriteRenderer>();

        //weaponSprite = transform.Find("Aim").Find("WeaponSprite");
        healthLoaded = false;
        state = State.Normal;
    }

    protected override void Start()
    {
        base.Start();
        moving = false;
        canRoll = true;
        rolledWall = false;
        scalar = new Vector3(.85f, .85f, .85f);
        immuneTime = 1.0f;
        velocity = new Vector3(0, 0, 0);
        DamageTaken();
    }
    
    private void Update()
    {
        if (!GameManager.instance.loadingScreen.activeInHierarchy && !MainMenu.inMainMenu)
        {
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && alive)
            {
                MainMovement();
                UpdateOpacity();
                UpdateManaBar();
                IsOnScreen();
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }

    public void IsOnScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
        {
            onScreen = true;
        }
        else
        {
            onScreen = false;
        }
    }


    private void UpdateOpacity()
    {
        if (alive)
        {
            if (!boxCollider.enabled || Time.time - lastImmune < immuneTime || isInvincible)
            {
                s0.color = new Color(1f, 1f, 1f, .4f);
                s1.color = new Color(1f, 1f, 1f, .4f);
                s2.color = new Color(1f, 1f, 1f, .4f);
                s3.color = new Color(1f, 1f, 1f, .4f);
            }
            else
            {
                s0.color = new Color(1f, 1f, 1f, 1f);
                s1.color = new Color(1f, 1f, 1f, 1f);
                s2.color = new Color(1f, 1f, 1f, 1f);
                s3.color = new Color(1f, 1f, 1f, 1f);
            }
        }
    }


    private void MainMovement()
    {
        if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && !MainMenu.inMainMenu)
        {
            holdingSpriteRenderer.sprite = GameManager.instance.holdingSprites[GameManager.instance.currentSkillIndex];
            if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
            {
                roundModifier = (1f + (.01f * GameManager.instance.completedWave));
                damageHealthTimer -= Time.deltaTime;
                if (damageHealthTimer < 0)
                {
                    if (healthBar.localScale.x < healthDamageBar.localScale.x)
                    {
                        float shrinkSpeed = .5f;
                        healthDamageBar.localScale = new Vector3((float)(0.9590346f * ((healthDamageBar.localScale.x) - (shrinkSpeed * Time.deltaTime))), 0.7565584f, 1);
                    }
                    UpdateBarText();
                }
                switch (state)
                {
                    case State.Normal:
                        boxCollider.enabled = true;
                        float x = Input.GetAxisRaw("Horizontal");
                        float y = Input.GetAxisRaw("Vertical");
                        moveDir = new Vector3(x, y).normalized;

                        if (Input.GetKeyDown(KeyCode.Space) && (moveDir != Vector3.zero))
                        {
                            if (canRoll)
                            {
                                rollDir = moveDir;
                                rollSpeed = 8f;
                                state = State.Rolling;
                            }
                        }
                        break;
                    case State.Rolling:
                        pushDirection = Vector3.zero;
                        if (rolledWall)
                        {
                            moveDir = moveDir / 1.05f;
                            rolledWall = false;
                            break;
                        }
                        boxCollider.enabled = false;
                        float rollSpeedDropMultiplier = 1.5f;
                        rollSpeed -= rollSpeed * rollSpeedDropMultiplier * Time.deltaTime;

                        float rollSpeedMinimum = 4f;
                        if (rollSpeed < rollSpeedMinimum)
                        {
                            state = State.Normal;
                        }
                        break;
                }
            }
            if (moveDir == Vector3.zero)
                moving = false;
            else
                moving = true;
        }
    }
   
    
    private void FixedUpdate()
    {
        if (!GameManager.instance.loadingScreen.activeInHierarchy && !MainMenu.inMainMenu)
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && alive)
            {
                StateForMovement();
                MainMovement();
                GameManager.instance.specialManager.DoSkill();
                RegenMana();
            }
            else
            {
                Time.timeScale = 0f;
            }
    }
    
    private void RegenMana()
    {
        if(canRegen)
        {
            if (manaAmount != maxMana)
            {
                manaAmount += 0.05f;
                if (manaAmount > maxMana)
                    manaAmount = maxMana;
            }
        }
    }

    private void StateForMovement()
    {
        if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && !MainMenu.inMainMenu)
        {
            switch (state)
            {
                case State.Normal:
                    {
                        UpdateMotor(moveDir * .7f);

                    }
                    break;
                case State.Rolling:
                    pushDirection = Vector3.zero;
                    UpdateMotor(moveDir * .7f, 1.5f, 1.5f, rollSpeed);
                    if (canRoll)
                    {
                        canRoll = false;
                        StartCoroutine(RollCooldownAndSound());
                    }
                    break;

            }

        }
    }

    IEnumerator RollCooldownAndSound()
    {

        GameManager.instance.PlayAudio(1);
        yield return new WaitForSeconds(rollCooldown / (float)(1f + (GameManager.instance.completedWave * .01)));
        //canDash = true;
        canRoll = true;
    }

    protected override void UpdateMotor(Vector3 input, float xSpeed = 1.5f, float ySpeed = 1.5f, float speed = 1.5f)
    {
        if (alive)
        {
            if (Pause.gameActive)
            {
                if (input != Vector3.zero && state == State.Normal)
                {
                    //GameManager.instance.PlayAudio(7);
                }
                velocity = Vector3.zero;
                // Reset MoveDelta
                if (state == State.Normal)
                {
                    moveDelta = input * speed * roundModifier;
                }
                else
                {
                    moveDelta = input * speed;
                }
                // Child transformation???
                //transform.GetChild(0).GetComponent<Transform>().localScale = ...


                // Add push vector, if any
                moveDelta += pushDirection;

                //Reduce push force every frame, based off recovery speed
                pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);


                // See if we can move in this direction by casting a box there. If the box returns null, means we can move there
                RaycastHit2D hity = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * 1.05), (float)(boxCollider.size.y * 1.05)), 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking", "PlayerBlocking", "AllBlocking"));
                if (hity.collider == null)
                {
                    // Movement
                    transform.Translate(0, (float)(moveDelta.y * Time.deltaTime), 0);
                    velocity += new Vector3(0, (float)(moveDelta.y * Time.deltaTime), 0);

                }
                else
                {
                    if ((hity.collider.tag != "Blocking") && (hity.collider.tag != "AllBlocking") && (hity.collider.tag != "PlayerBlocking"))
                    {
                        transform.Translate(0, (float)(moveDelta.y * Time.deltaTime), 0);
                        velocity += new Vector3(0, (float)(moveDelta.y * Time.deltaTime), 0);

                    }
                    else
                    {
                        rolledWall = true;

                    }
                }



                RaycastHit2D hitx = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * 1.05), (float)(boxCollider.size.y * 1.05)), 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking", "PlayerBlocking", "AllBlocking"));
                if (hitx.collider == null)
                {
                    // Movement
                    transform.Translate((float)(moveDelta.x * Time.deltaTime), 0, 0);
                    velocity += new Vector3((float)(moveDelta.x * Time.deltaTime), 0, 0);
                }
                else
                {
                    if ((hitx.collider.tag != "Blocking") && (hitx.collider.tag != "AllBlocking") && (hitx.collider.tag != "PlayerBlocking"))
                    {
                        transform.Translate((float)(moveDelta.x * Time.deltaTime), 0, 0);
                        velocity += new Vector3((float)(moveDelta.x * Time.deltaTime), 0, 0);

                    }
                    else
                    {
                        rolledWall = true;

                    }
                }

                velocity = velocity.normalized;

                if (hitx.collider != null || hity.collider != null)
                {
                    pushDirection = Vector3.zero;
                }
            }
        }
        else
        {
            // Remember to re-enable when reviving
            boxCollider.enabled = false;
        }
    }

    protected override void RecieveDamage(Damage dmg)
    {
        if (alive && !GameManager.instance.spawnerManager.waveOver)
        {
            if (Time.time - lastImmune > immuneTime)
            {
                if (isInvincible)
                {
                    if (!GameManager.instance.spawnerManager.waveOver)
                    {
                        GameManager.instance.specialManager.ManaWorks();
                        GameManager.instance.specialManager.interacted = true;
                        lastImmune = Time.time;
                        GameManager.instance.hud.UpdateHUD();
                        manaAmount -= 10f;
                    }
                    GameManager.instance.PlayAudio(10);
                    return;
                }
                GameManager.instance.OneTimeScreenTint(0);
                lastImmune = Time.time;
                hitpoint -= dmg.damageAmount;
                DamageTaken();
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 60, Color.red, transform.position, Vector3.zero, 0.5f);
                GameManager.instance.hud.UpdateHUD();
                GameManager.instance.hud.DamageTaken();

                if (hitpoint <= 0)
                {
                    spriteAnim.SetTrigger("hit");
                    StartCoroutine(Sound());
                    hitpoint = 0;
                    Death();
                }
                else
                {
                    spriteAnim.SetTrigger("hit");
                    StartCoroutine(Sound());
                }
            }
        }
    }

    public void UpdateManaBar()
    {
        // Health Bar
        float currMana = manaAmount;
        float maxM = maxMana;
        float manaCompletionRatio = (float)currMana / (float)maxM;

        if (currMana == maxM)
        {
            manaBar.localScale = new Vector3(0.9590346f, 0.7565584f, 1f);
        }
        else
        {
            manaBar.localScale = new Vector3(manaCompletionRatio, 0.7565584f, 1);
        }
    }

    public void RefreshHealthBar()
    {
        if (Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            healthBar.localScale = new Vector3(0.9590346f, 0.7565584f, 1);
        }
        UpdateBarText();
    }

    public void UpdateBarText()
    {
        int curr = hitpoint;
        int max = maxHitpoint;
        healthBarText.text = curr + "/" + max;
    }

    public void DamageTaken()
    {
        // Health Bar
        int currHealth = hitpoint;
        int maxHealth = maxHitpoint;
        float healthCompletionRatio = (float)currHealth / (float)maxHealth;

        damageHealthTimer = MAX_DAMAGE_HEALTH_TIMER;

        if (currHealth == maxHealth)
        {
            healthBar.localScale = new Vector3(0.9590346f, 0.7565584f, 1f);
        }
        else
        {
            healthBar.localScale = new Vector3(healthCompletionRatio, 0.7565584f, 1);
        }
        UpdateBarText();
    }

    IEnumerator Sound()
    {
        GameManager.instance.PlayAudio(0);
        yield return new WaitForSeconds(1.5f);
    }

    public void SwapSprite(int skinID)
    {
        spriteRenderer.sprite = GameManager.instance.playerSprites[skinID];
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
    
    public void OnLevelUp()
    {
        GameManager.instance.PlayAudio(9);
        GameManager.instance.ShowText("LEVEL UP", 80, Color.magenta, transform.position, Vector3.up * 45, 1.5f);
        maxHitpoint += 10;
        hitpoint = maxHitpoint;
        GameManager.instance.hud.HealTaken();
        maxMana += 2;
        manaAmount = maxMana;
        GameManager.instance.hud.UpdateManaBar();
        RefreshHealthBar();
    }
    
    public void SetLevel(int level)
    {
        if (!healthLoaded)
        {
            for (int i = 1; i < level; i++)
            {
                maxHitpoint += 20;
                hitpoint = maxHitpoint;
            }
            healthLoaded = true;
        }
    }

    protected override void Death()
    {
        GameManager.instance.spawnerManager.canceled = true;
        GameManager.instance.spawnerManager.canceled = true;
        alive = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Background").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("InnerBackground").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Progress").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("DamageProgress").GetComponent<SpriteRenderer>().enabled = false;
        GameManager.instance.hud.healthBar.localScale = new Vector3(0, 1, 1);
        GameManager.instance.hud.healthText.text = "0 / " + maxHitpoint.ToString();
        GameManager.instance.rMenu.anim.SetTrigger("show");
        Time.timeScale = 0;

    }

    public void Revive()
    {
        alive = true;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Background").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("InnerBackground").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Progress").GetComponent<SpriteRenderer>().enabled = true;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("DamageProgress").GetComponent<SpriteRenderer>().enabled = true;
        transform.position = GameManager.instance.currentSpawnPoint;
        moveDelta = Vector3.zero;
        velocity = Vector3.zero;
        pushDirection = Vector3.zero;
        Time.timeScale = 1;
        GameManager.instance.player.RefreshHealthBar();
        GameManager.instance.hud.HealTaken(GameManager.instance.player.maxHitpoint);
    }


}
