using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class Enemy : Mover
{
    // Experience
    public int xpValue = 1;
    public int coinValue = 2;

    // Logic
    public float triggerLength = (float)1.5;
    public float chaseLength = (float)2.5;
    public bool chasing;
    protected bool collidingWithPlayer;
    protected Transform playerTransform;
    protected Vector3 startingPosition;
    public int damage = 0;
    public float slowModifier = 1f;
    public bool isSlowed = false;
    public bool maxSlowed = false;
    public int numSlows = 0;
    public float dmgInc = 1.1f;
    public bool incDmg = false;
    public float baseSpeed = 1f;

    // Sound
    public AudioClip enemyHitSound;
    public AudioClip enemyDeathSound;
    public AudioClip skinHitSound;
    public AudioSource enemyHit;
    public AudioSource enemyDeath;
    public AudioSource skinHit;

    // Animation
    private Animator anim;
    public List<Sprite> enemySprites;

    // Health Bar
    public Transform healthBar;
    public Transform healthDamageBar;
    public Text healthBarHP;
    public Text healthBarHPBack;
    protected float damageHealthTimer;
    protected const float MAX_DAMAGE_HEALTH_TIMER = .3f;
    public Text slowText;
    public Text slowTextUnder;
    public Image slowImage;
    public bool boss = false;
    public float pathingSpeed;
    public float originalPathingSpeed;

    // Hit Box
    public ContactFilter2D filter;
    protected BoxCollider2D hitbox;
    protected Collider2D[] hits = new Collider2D[10];
    protected SpriteRenderer spriteRenderer;
    protected SpriteRenderer minimapIcon;

    // Projectile
    public EnemyThrowable throwing;
    protected bool canThrow;
    protected float pitchAudio;
    public float manualPitch = 0;

    public bool onScreen;

    protected void Awake()
    {
        spriteRenderer = transform.Find("EnemySprite").GetComponent<SpriteRenderer>();
        hitbox = transform.Find("Hitbox").GetComponent<BoxCollider2D>();
        anim = transform.Find("EnemySprite").GetComponent<Animator>();
        minimapIcon = transform.Find("MinimapIcon").GetComponent<SpriteRenderer>();
//         healthDamageBarImage = transform.Find("HealthBar").Find("HealthBarContainer").Find("DamageProgress").GetComponent<SpriteRenderer>();


    }

    protected override void Start()
    {
        base.Start();
        originalPathingSpeed = transform.GetComponent<AIPath>().maxSpeed;
        pathingSpeed = transform.GetComponent<AIPath>().maxSpeed + (0.0035f * GameManager.instance.completedWave);
        transform.GetComponent<AIPath>().maxSpeed = pathingSpeed;
        slowModifier = 1f;
        damage = Mathf.RoundToInt((GameManager.instance.enemyDamages[0] * (float)(1 + GameManager.instance.completedWave/100f)));
        coinValue = Mathf.RoundToInt((coinValue * (float)(1 + GameManager.instance.completedWave / 10f)));
        xpValue = Mathf.RoundToInt((xpValue * (float)(1 + GameManager.instance.completedWave / 10f)));
        maxHitpoint = Mathf.RoundToInt((maxHitpoint * (float)(1 + (GameManager.instance.completedWave)/10f)));
        hitpoint = maxHitpoint;
        playerTransform = GameManager.instance.player.transform;
        startingPosition = transform.position;
        DamageTaken();
        if (!boss)
        {
            UpdateSlow();
        }
        // Gets hitbox instead of the collision box:
        if (manualPitch == 0)
        {
            pitchAudio = Random.Range(.8f, 1.2f);
            skinHit.pitch = pitchAudio;
            enemyDeath.pitch = pitchAudio;
            enemyHit.pitch = pitchAudio;
        }
        else
        {
            enemyDeath.pitch = manualPitch;
            enemyHit.pitch = manualPitch;
        }

    }

    protected virtual void Update()
    {
        if (MainMenu.inMainMenu)
        {
            Destroy(gameObject);
        }
        if (!GameManager.instance.loadingScreen.activeInHierarchy && !MainMenu.inMainMenu)
        {
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive && alive)
            {
                IsOnScreen();
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

    public virtual void SlowAdjust()
    {
        if (isSlowed && numSlows < 5)
        {
            pathingSpeed = pathingSpeed * .91f;
            transform.GetComponent<AIPath>().maxSpeed = pathingSpeed;
            isSlowed = false;
            numSlows++;
            UpdateSlow();
        }
    }


    public virtual void UpdateSlow()
    {
        if (alive)
        {
            if (numSlows == 0)
            {
                slowText.enabled = false;
                slowTextUnder.enabled = false;
                slowImage.enabled = false;
            }
            else
            {
                slowText.enabled = true;
                slowTextUnder.enabled = true;
                slowImage.enabled = true;
                slowText.text = numSlows.ToString();
                slowTextUnder.text = numSlows.ToString();
            }
        }

    }


    public virtual void Healthbar()
    {
        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            damageHealthTimer -= Time.deltaTime;
            if (damageHealthTimer < 0)
            {
                if (healthBar.localScale.x < healthDamageBar.localScale.x)
                {
                    float shrinkSpeed = .5f;
                    healthDamageBar.localScale = new Vector3((float)(0.9590346f * ((healthDamageBar.localScale.x) - (shrinkSpeed * Time.deltaTime))), 0.7565584f, 1);
                }
            }
        }
    }

    public bool PlayerInRange()
    {
        if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
            if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                return true;
        
        return false;
    }

    protected virtual void FixedUpdate()
    {
        if (alive)
        {
            if (Pause.gameActive)
            {
                /* // Is the player in range?
                 if (Vector3.Distance(playerTransform.position, startingPosition) < chaseLength)
                 {
                     if (Vector3.Distance(playerTransform.position, startingPosition) < triggerLength)
                         chasing = true;

                     if (chasing)
                     {
                         if (!collidingWithPlayer)
                         {
                            // UpdateMotor((playerTransform.position - transform.position).normalized, .85f, .85f);
                         }
                     }
                     else
                     {
                         //UpdateMotor(startingPosition - transform.position, 2.2f, 2.2f);
                     }
                 }
                 else
                 {
                   //  UpdateMotor(startingPosition - transform.position, 2.2f, 2.2f);
                     chasing = false;
                 }

                 // Check for overlap
                 collidingWithPlayer = false;*/
                if (alive)
                {
                    Healthbar();
                    if (!boss)
                    {
                        SlowAdjust();
                    }
                    if (pushDirection != Vector3.zero)
                    {
                        transform.GetComponent<AIPath>().enabled = false;
                        UpdateMotor(pushDirection, 4f, 4f);
                    }
                    else
                    {

                        transform.GetComponent<AIPath>().enabled = true;

                    }
                }


                boxCollider.OverlapCollider(filter, hits);
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i] == null)
                        continue;

                    if (hits[i].tag == "Fighter" && hits[i].name == "Player")
                    {
                        collidingWithPlayer = true;
                    }

                    // The array is not cleaned up, so we clean it
                    hits[i] = null;
                }
            }
        }
    }

    protected override void RecieveDamage(Damage dmg)
    {
        if (alive)
        {
            if (Time.time - lastImmuneDmg > immuneTimeDmg)
            {
                if (incDmg)
                {
                    dmg.damageAmount = (int)(dmg.damageAmount * dmgInc);
                }
                lastImmuneDmg = Time.time;
                hitpoint -= dmg.damageAmount;
                DamageTaken();
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 45, Color.red, transform.position, Vector3.zero, 0.5f);

                if (hitpoint <= 0)
                {
                    hitpoint = 0;
                    Death();
                }

            }
            if (hitpoint > 0)
            {
                if (Time.time - lastImmune > immuneTime)
                {
                    lastImmune = Time.time;
                    pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;
                    anim.SetTrigger("hit");

                    if (!skinHit.isPlaying)
                    {
                        skinHit.PlayOneShot(skinHitSound, GameManager.instance.sfxSound);
                    }

                    if (!enemyHit.isPlaying)
                    {
                        enemyHit.PlayOneShot(enemyHitSound, GameManager.instance.sfxSound);
                    }
                }
                else
                {
                    anim.SetTrigger("immune");
                }
            }

        }

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
            healthBar.localScale = new Vector3 (0.9590346f, 0.7565584f, 1f);
        }
        else
        {
            healthBar.localScale = new Vector3(healthCompletionRatio, 0.7565584f, 1);
        }
        healthBarHP.text = Mathf.RoundToInt(currHealth) + "/" + Mathf.RoundToInt(maxHealth);
        healthBarHPBack.text = Mathf.RoundToInt(currHealth) + "/" + Mathf.RoundToInt(maxHealth);

    }


    public bool IsChasing()
    {
        return chasing;
    }
    protected override void UpdateMotor(Vector3 input, float xSpeed = 1.4f, float ySpeed = 1.4f, float speed = 1f)
    {
        if (alive)
        {
            if (Pause.gameActive)
            {
                // Reset MoveDelta
                //float roundModifier = (1f + (.01f * GameManager.instance.completedWave));
                // moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0).normalized * speed * roundModifier * slowModifier * baseSpeed;
                moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0).normalized * xSpeed;
                // Child transformation???
                //transform.GetChild(0).GetComponent<Transform>().localScale = ...


                /*// Swap spite direction
                if (moveDelta.x > 0)
                    transform.localScale = Vector3.one;
                else if (moveDelta.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);*/

                // Add push vector, if any
                moveDelta += pushDirection;

                //Reduce push force every frame, based off recovery speed
                pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

                // See if we can move in this direction by casting a box there. If the box returns null, means we can move there
                RaycastHit2D hity = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * .90), (float)(boxCollider.size.y * .90)), 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Blocking", "EnemyBlocking", "AllBlocking", "Actor"));
                if (hity.collider == null)
                {
                    // Movement
                    transform.Translate(0, (float)(moveDelta.y * Time.deltaTime * 0.6), 0);

                }
                else
                {
                    if ((hity.collider.tag != "Blocking") && (hity.collider.tag != "AllBlocking") && (hity.collider.tag != "EnemyBlocking") && (hity.collider.tag != "Fighter"))
                    {
                        if (hity.collider.tag == "Fighter")
                        {
                            transform.Translate(0, (float)(Random.Range(-moveDelta.y / 4, moveDelta.y / 4) * Time.deltaTime * 0.6), 0);
                        }
                        transform.Translate(0, (float)(moveDelta.y * Time.deltaTime * 0.6), 0);

                    }

                }



                RaycastHit2D hitx = Physics2D.BoxCast(transform.position, new Vector2((float)(boxCollider.size.x * .90), (float)(boxCollider.size.y * .90)), 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Blocking", "EnemyBlocking", "AllBlocking", "Actor"));
                if (hitx.collider == null)
                {
                    // Movement
                    transform.Translate((float)(moveDelta.x * Time.deltaTime * 0.6), 0, 0);
                }
                else
                {
                    if ((hitx.collider.tag != "Blocking") && (hitx.collider.tag != "AllBlocking") && (hitx.collider.tag != "EnemyBlocking") && (hitx.collider.tag != "Fighter"))
                    {
                        if (hitx.collider.tag == "Fighter")
                        {
                            pushDirection = (transform.position - new Vector3(transform.position.x - Random.Range(-.15f, .15f), transform.position.y - Random.Range(-.15f, .15f))).normalized * 2f;
                        } 
                        else
                        {
                            transform.Translate((float)(moveDelta.x * Time.deltaTime * 0.6), 0, 0);
                        }

                    }
                }

                if (hitx.collider != null || hity.collider != null)
                {
                    pushDirection = Vector3.zero;
                }
            }
        }
    }


    protected override void Death()
    {
        DisableHealthbar();
        DisableEnemy();
        alive = false;
        StartCoroutine(DeathAnimAndSound());
        RewardPlayer();

    }

    protected void DisableEnemy()
    {
        transform.GetComponent<AIPath>().enabled = false;
        hitbox.enabled = false;
        boxCollider.enabled = false;
        transform.Find("Hitbox").GetComponent<BoxCollider2D>().enabled = false;
        transform.Find("Collidebox").GetComponent<BoxCollider2D>().enabled = false;
    }

    protected void DisableHealthbar()
    {
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Background").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("InnerBackground").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Progress").GetComponent<SpriteRenderer>().enabled = false; ;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("DamageProgress").GetComponent<SpriteRenderer>().enabled = false;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("Canvas").Find("Text").GetComponent<Text>().enabled = false; ;
        transform.Find("HealthBar").Find("HealthBarContainer").Find("CanvasUnder").Find("Background").GetComponent<Text>().enabled = false;
        if(!boss)
        {
            slowText.enabled = false;
            slowTextUnder.enabled = false;
            slowImage.enabled = false;
        }
    }

    protected void RewardPlayer()
    {
        GameManager.instance.GrantXP(xpValue);
        GameManager.instance.coins += coinValue;
        GameManager.instance.CoinsSound();
        GameManager.instance.hud.UpdateHUD();
        GameManager.instance.ShowText("+" + xpValue + " exp", 30, Color.magenta, transform.position, Vector3.up * 40, 1.0f);
        GameManager.instance.ShowText("+" + coinValue + " coins", 30, Color.yellow, new Vector3(transform.position.x, (float)(transform.position.y + 0.07), 0), Vector3.up * 40, 1.0f);
    }

    IEnumerator DeathAnimAndSound()
    {
        if (!skinHit.isPlaying)
        {
            skinHit.PlayOneShot(skinHitSound, GameManager.instance.sfxSound);
        }

        if (!enemyDeath.isPlaying)
        {
            enemyDeath.PlayOneShot(enemyDeathSound, GameManager.instance.sfxSound);
        }
        minimapIcon.enabled = false;
        anim.SetTrigger("killed");
        yield return new WaitForSeconds(0.7f);
        anim.SetTrigger("killedGround");
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
