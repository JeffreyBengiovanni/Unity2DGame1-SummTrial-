using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    
    public RectTransform xpBar;
    public Text xpText;
    public Text levelText;
    public Text coinText;
    public Text roundText;
    public Text enemiesText;
    public Text timeText;
    public Text skillText;
    public Image skillImage;
    public Text dashText;
    public Text skillCooldownText;

    public Text specialText;
    public Image specialImage;

    public Text targetText;
    public Image targetImage;

    public RectTransform manaBar;
    public Text manaText;

    public RectTransform healthBar;
    public Text healthText;
    public RectTransform healthDamageBar;
//     private Image healthDamageBarImage;
    private float damageHealthTimer;
    private const float MAX_DAMAGE_HEALTH_TIMER = 1f;
    private bool go;
//     private Color damagedColor;
    public static HUD objectInstance;
    public float currentTime, dashCooldown, skillCurrentTime, skillCooldown;



    private void Awake()
    {
        DontDestroyOnLoad(this);
        if (objectInstance == null)
        {
            objectInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DamageTaken();
        HealTaken();
        go = true;
    }

    private void Update()
    {
        /*if (damagedColor.a > 0)
        {
            damageHealthTimer -= Time.deltaTime;
            if (damageHealthTimer < 0)
            {
                float fadeAmount = 5f;
                damagedColor.a -= fadeAmount * Time.deltaTime;
                healthDamageBarImage.color = damagedColor;
            }
        }*/
        if (go)
        {
            go = false;
            UpdateHUD();
            dashCooldown = GameManager.instance.player.rollCooldown;
            skillCooldown = GameManager.instance.currentSkill.GetSpeed();
        }

        RollCooldown();

        SkillCooldown();

        HealbarUpdate();

    }

    public void UpdateManaBar()
    {
        float totalMana = GameManager.instance.player.maxMana;
        float currentMana = GameManager.instance.player.manaAmount;

        float manaCompletionRatio = (float)currentMana / (float)totalMana;

        if (currentMana == totalMana)
        {
            manaBar.localScale = Vector3.one;
        }
        else
        {
            manaBar.localScale = new Vector3(manaCompletionRatio, 1, 1);
        }

        // Meta
        manaText.text = ((int)currentMana).ToString() + " / " + ((int)totalMana).ToString();

    }

    public void HealbarUpdate()
    {
        damageHealthTimer -= Time.deltaTime;
        if (damageHealthTimer < 0)
        {
            if (healthBar.localScale.x < healthDamageBar.localScale.x)
            {
                float shrinkSpeed = .5f;
                healthDamageBar.localScale = new Vector3((float)(healthDamageBar.localScale.x) - (shrinkSpeed * Time.deltaTime), 1, 1);
            }
        }
        UpdateHUD();
    }

    public void RollCooldown()
    {
        bool c = GameManager.instance.player.GetComponent<PlayerAimWeapon>().cooldownDone;
        if (!c)
        {
            skillCurrentTime = skillCurrentTime - Time.deltaTime;
            if (skillCurrentTime < 0)
            {
                skillCooldownText.color = Color.green;
                skillCooldownText.text = "READY";
                skillCooldownText.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                skillCooldownText.color = Color.red;
                skillCooldownText.text = "     CD: " + ((float)skillCurrentTime).ToString("F1");
                skillCooldownText.alignment = TextAnchor.MiddleLeft;
            }

        }
        else
        {
            skillCurrentTime = GameManager.instance.currentSkill.GetSpeed();
            skillCooldownText.color = Color.green;
            skillCooldownText.text = "READY";
            skillCooldownText.alignment = TextAnchor.MiddleCenter;
        }
    }

    public void SkillCooldown()
    {
        if (!GameManager.instance.player.canRoll)
        {
            currentTime = currentTime - Time.deltaTime;
            if (currentTime < 0)
            {
                dashText.color = Color.green;
                dashText.text = "READY";
                dashText.alignment = TextAnchor.MiddleCenter;
            }
            else
            {
                dashText.color = Color.red;
                dashText.text = "     CD: " + (currentTime).ToString("F1");
                dashText.alignment = TextAnchor.MiddleLeft;
            }

        }
        else
        {
            currentTime = dashCooldown / (float)(1f + (GameManager.instance.completedWave * .01));
            dashText.color = Color.green;
            dashText.text = "READY";
            dashText.alignment = TextAnchor.MiddleCenter;
        }
    }


    public void DamageTaken()
    {
        // Health Bar
        int currHealth = GameManager.instance.player.hitpoint;
        int maxHealth = GameManager.instance.player.maxHitpoint;
        float healthCompletionRatio = (float)currHealth / (float)maxHealth;

        /*// Damage Health Bar
        if (damagedColor.a <= 0)
        {
            // Damage bar image is invisible
            healthDamageBar.localScale = healthBar.localScale;
        }
        damagedColor.a = 1;
        healthDamageBarImage.color = damagedColor;
        damageHealthTimer = MAX_DAMAGE_HEALTH_TIMER;*/
        damageHealthTimer = MAX_DAMAGE_HEALTH_TIMER;

        if (currHealth == maxHealth)
        {
            healthBar.localScale = Vector3.one;
        }
        else
        {
            healthBar.localScale = new Vector3(healthCompletionRatio, 1, 1);
        }

        // Meta
        healthText.text = (currHealth).ToString() + " / " + (maxHealth).ToString();
    }
   
    public void HealTaken(int increase = 0)
    {
        // Health Bar
        int currHealth = GameManager.instance.player.hitpoint + increase;
        int maxHealth = GameManager.instance.player.maxHitpoint;
        if(currHealth > maxHealth)
        {
            currHealth = maxHealth;
            GameManager.instance.player.hitpoint = maxHealth;
        }
        else
        {
            GameManager.instance.player.hitpoint = currHealth;
        }

        float healthCompletionRatio = (float)currHealth / (float)maxHealth;


        if (currHealth == maxHealth)
        {
            healthBar.localScale = Vector3.one;
            GameManager.instance.player.healthBar.localScale = new Vector3(0.9590346f, 0.7565584f, 1);

        }
        else
        {
            healthBar.localScale = new Vector3(healthCompletionRatio, 1, 1);
            GameManager.instance.player.healthBar.localScale = new Vector3((float)(0.9590346f * (healthCompletionRatio)), 0.7565584f, 1);

        }

        healthDamageBar.localScale = healthBar.localScale;

        // Meta
        healthText.text = (currHealth).ToString() + " / " + (maxHealth).ToString();
    }

    public void UpdateTargetToggle()
    {
        if (GameManager.instance.cameraMotor.targetLock)
        {
            targetText.text = "ENABLED";
            targetText.color = Color.green;
            targetImage.color = Color.red;
        }
        else
        {
            targetText.text = "DISABLED";
            targetText.color = Color.red;
            targetImage.color = new Color(Color.red.r, Color.red.b, Color.red.g, 0.5f);
        }
    }

    public void UpdateHUD()
    {
        UpdateManaBar();
        UpdateTargetToggle();
        levelText.text = "Level " + GameManager.instance.GetCurrentLevel().ToString();
        coinText.text = "Coins: " + GameManager.instance.coins.ToString();
        roundText.text = "Round: " + GameManager.instance.currentWave;
        timeText.text = GameManager.instance.timeManager.GetTime();
        skillText.text = GameManager.instance.currentSkill.GetName();
        skillImage.sprite = GameManager.instance.holdingSprites[GameManager.instance.currentSkillIndex];
        specialText.text = GameManager.instance.specialManager.names[GameManager.instance.specialManager.currentSkillindex];
        specialImage.sprite = GameManager.instance.specialSprites[GameManager.instance.specialManager.currentSkillindex];

        if (!GameManager.instance.spawnerManager.waveOver)
        {
            enemiesText.color = Color.red;
            enemiesText.text = "Alive: " + GameManager.instance.spawnerManager.numEnemies;
        }
        else 
        {
            if (!GameManager.instance.player.alive)
            {
                enemiesText.color = Color.red;
                enemiesText.text = "Round Failed";
            }
            else if (GameManager.instance.completedWave == 0)
            {
                enemiesText.text = "";
            }
            else
            {
                GameManager.instance.player.hitpoint = GameManager.instance.player.maxHitpoint;
                GameManager.instance.hud.HealTaken();
                enemiesText.color = Color.green;
                enemiesText.text = "Round Completed";
            }
        }


        // XP Bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if (currLevel == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevLevelXP = GameManager.instance.GetXPToLevel(currLevel - 1);
            int currLevelXP = GameManager.instance.GetXPToLevel(currLevel);

            int diff = currLevelXP - prevLevelXP;
            int currXPIntoLevel = GameManager.instance.experience - prevLevelXP;

            float xpCompletionRatio = (float)currXPIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(xpCompletionRatio, 1, 1);
            xpText.text = currXPIntoLevel.ToString() + " / " + diff;

        }
    }
}
