using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    // Resources
    public List<Sprite> playerSprites;
    public List<Sprite> playerMovingSprites;
    public List<Sprite> weaponSprites;
    public List<Sprite> holdingSprites;
    public List<Sprite> specialSprites;
    public List<int> weaponPrices;
    public List<int> xpTable;
    public List<int> madeUpgrades;
    public List<AudioClip> soundDirectory;


    // Enemy Damage Globals
    public List<int> enemyDamages;
    // 1 - Slime
    // 2 - BigSlime


    // ~~~~~~~~~~~~~Skills~~~~~~~~~~~~~
    public List<Skill> skills;
    public Skill currentSkill;
    public int currentSkillIndex;

    // Skill List
    public Skill scythe;
    public Skill fireball;
    public Skill minigun;
    public Skill shatter;
    public Skill iceSpike;
    public Skill vortex;
    public Skill lightning;
    public Skill sever;
    public Skill spark;

    // References;
    public Player player;
    public ThrowingWeapon weapon;
    public FloatingTextManager floatingTextManager;
    public Pause pause;
    public SkillPause sPause;
    public HUD hud;
    public SkillMenu sMenu;
    public WeaponTesting weaponTesting;
    public MainMenu mainMenu;
    public SettingsMenu settingsMenu;
    public DialogueMenu dialogueMenu;
    public DialoguePause dPause;
    public ReviveMenu rMenu;
    public SpawnerManager spawnerManager;
    public TimeManager timeManager;
    public Transform credits;
    public GameObject loadingScreen;
    public Special specialManager;
    public MapSelect mapSelect;
    public CameraMotor cameraMotor;


    // More Loading
    public RectTransform loadingBar;
    public Text loadingBarText;


    private List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    // Logic
    public int coins = 50;
    public int experience;
    public int preferredSkin;
    public int charLevel;
    public Vector3 currentSpawnPoint;
    public int currentWave;
    public int completedWave;
    public bool healthStartup = true;
    public bool maximized;
    public float totalSceneProgress;

    // Sound
    public AudioClip coinsGainSound;
    private AudioSource coinsGain;
    public float sfxSound;
    public float musicSound;
    public AudioClip[] audioClips;
    public AudioSource[] audioSources;
    public AudioClip[] musicFiles;
    public HashSet<int> usedMusic;
    public int currentMusicIndex = -1;

    public Animator[] screenTints;
    public bool[] screenTintsTriggered;

    public Sprite[] mapPreviews;
    public string[] mapNames;

    public Animator creditsAnim;

    // Fonts
    public Font f1;
    public Font f2;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        //SceneManager.LoadSceneAsync(0, LoadSceneMode.Additive);
        loadingBarText.text = "0%";
        loadingBar.localScale = new Vector3(0, 1, 1);
        loadingScreen.gameObject.SetActive(false);

        healthStartup = true;

        maximized = true;

        // Load Skills
        // Skills

        currentSkillIndex = 1;
        scythe = new Skill
        {
            damageChart = new List<int> { 8, 10, 12, 14, 16, 18 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 3, 6, 9 },
            sizePrices = new List<int> { 400, 800 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 800, 1600 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .25f, .5f, .75f, 1.0f, 1.25f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2f,
            scale = 1f,
            velocity = 1.25f,
            skillSprite = weaponSprites[0],
            skillName = "SCYTHE",
            skilLDescription = "The SCYTHE skill is often used due to its strong pierce ability. " +
            "This means that the SCYTHE can hit more enemies with a single scythe than other skills can. " +
            "On top of its ability to pierce, the SCYTHE offers decent knockback and damage as well. To fully" +
            " take advantage of this weapon, it should be used against large groups of enemies.",
            duration = 1.25f,
            unlocked = 1,
            baseKnockback = 3f,
            pierce = 3,
            projectiles = 1,
            windup = 0f
        };

        fireball = new Skill
        {
            damageChart = new List<int> { 4, 6, 8, 10, 12, 14 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 1, 2, 3 },
            sizePrices = new List<int> { 400, 800 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 800, 1600 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .2f, .4f, .6f, .8f, 1.0f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2.25f,
            scale = 1f,
            velocity = 1.5f,
            skillSprite = weaponSprites[1],
            skillName = "FIREBALL",
            skilLDescription = "The FIREBALL skill is renowned for its area damage. Although it may not be a very " +
            "fast weapon, it makes up for that with its ability to explode on contact. This explosion does damage to " +
            "every enemy within its area. This skill offers some knockback and moderate damage, but shines when used " +
            "against grouped enemies.",
            duration = 1.1f,
            unlocked = 1,
            baseKnockback = 1.75f,
            pierce = 1,
            projectiles = 1,
            windup = 0f
        };

        minigun = new Skill
        {
            damageChart = new List<int> { 2, 4, 6, 8, 10 },
            damagePrices = new List<int> { 50, 100, 200, 400 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 1, 2, 3 },
            sizePrices = new List<int> { 400, 800 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 800, 1600 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .05f, .10f, .15f, .2f, .25f},
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = .5f,
            scale = 1f,
            velocity = 3.75f,
            skillSprite = weaponSprites[2],
            skillName = "MINIGUN",
            skilLDescription = "The MINIGUN is a skill that fires bullets at an incredible rate. " +
            "Though it lacks strong damage and knockback, it is a consistent weapon that can hit its " +
            "target quickly and often. This skill shines when something is fast and evasive because of " +
            "its bullet velocity and fire speed.",
            duration = .55f,
            unlocked = 1,
            baseKnockback = 1f,
            pierce = 1,
            projectiles = 1,
            windup = 0f
        };

        shatter = new Skill
        {
            damageChart = new List<int> { 4, 6, 8, 10, 12, 14 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 1, 2, 3 },
            sizePrices = new List<int> { 400, 800 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 800, 1600 },
            velocityChart = new List<float> { 0f, .10f, .2f, .3f, .4f, .5f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .10f, .2f, .3f, .4f, .5f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2.25f,
            scale = 1f,
            velocity = 1.5f,
            skillSprite = weaponSprites[3],
            skillName = "SHATTER",
            skilLDescription = "The SHATTER skill summons a shard the splits into two when it" +
            " hits an enemy. What makes this skill very strong is its ability to stack damage with its" +
            " shattered pieces. These pieces can hit the same target the main shard collider with resulting" +
            " in strong damage. Upgrades to pierce cause this skill to split multiple times from a single" +
            " shard.",
            duration = 1.1f,
            unlocked = 1,
            baseKnockback = 1f,
            pierce = 1,
            projectiles = 1,
            windup = 0f
        };

        iceSpike = new Skill
        {
            damageChart = new List<int> { 4, 6, 8, 10, 12, 14 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 1, 2, 3, 4 },
            sizePrices = new List<int> { 300, 600, 1200 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 600, 800 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .4f, .8f, 1.2f, 1.6f, 2.0f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2.25f,
            scale = 1f,
            velocity = 3f,
            skillSprite = weaponSprites[4],
            skillName = "ICE SPIKE",
            skilLDescription = "The ICE SPIKE skill excels at crowd control compared to the other skills. It " +
            "slows enemies down by 20% and has very strong knockback. Combining these two can lead to an effective " +
            "lock down on a targets movement and position with upgrades. This spike from this skill is also shot at a " +
            "faster velocity than usual.",
            duration = .65f,
            unlocked = 1,
            baseKnockback = 4f,
            pierce = 1,
            projectiles = 1,
            windup = 0f
        };

        vortex = new Skill
        {
            damageChart = new List<int> { 4, 6, 8, 10, 12, 14 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 6, 9, 12, 15 },
            sizePrices = new List<int> { 300, 600, 1200 },
            projectilesChart = new List<int> { 1, 2, 3 },
            projectilesPrices = new List<int> { 500, 1000 },
            velocityChart = new List<float> { 0f, -.05f, -.1f, -.15f, -.2f, -.25f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .2f, .4f, .6f, .8f, 1.0f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2.5f,
            scale = 1f,
            velocity = .5f,
            skillSprite = weaponSprites[5],
            skillName = "VORTEX",
            skilLDescription = "The VORTEX skill is a unique skill. The same vortex can hit the same enemy " +
            "multiple times. This cooldown before it can hit again scales down with a decrease in cast speed. " +
            "The VORTEX skill also has upgrades to velocity slow down the projectile to maximize damage potential. " +
            "Pierce upgrades increase the number of enemies allowed in the vortex before it vanishes. ",
            duration = 2f,
            unlocked = 1,
            baseKnockback = 5.5f,
            pierce = 6,
            projectiles = 1,
            windup = 0f
        };

        lightning = new Skill
        {
            damageChart = new List<int> { 3, 5, 7, 9, 11, 13 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 3, 5, 7, 9 },
            sizePrices = new List<int> { 300, 600, 1200 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 600, 800 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .2f, .4f, .6f, .8f, 1.0f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 1.5f,
            scale = 1f,
            velocity = 4f,
            skillSprite = weaponSprites[6],
            skillName = "LIGHTNING BOLT",
            skilLDescription = "The LIGHTNING BOLT skill uses high velocity projectiles to hit far enemies away accurately. These" +
            " fast lightning bolts can be released at a quick interval to overwhelm enemies. The lightning bolt can" +
            " pierce through multiple targets for each bolt. This effect makes this skill shine when enemies are lined " +
            "up with each other.",
            duration = .5f,
            unlocked = 1,
            baseKnockback = 3.5f,
            pierce = 3,
            projectiles = 1,
            windup = 0f
        };

        sever = new Skill
        {
            damageChart = new List<int> { 7, 9, 11, 13, 15, 17 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 2, 4, 6, 8 },
            sizePrices = new List<int> { 300, 600, 1200 },
            projectilesChart = new List<int> { 1, 2, 3, 4, 5 },
            projectilesPrices = new List<int> { 200, 400, 600, 800 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .2f, .4f, .6f, .8f, 1.0f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = 2.25f,
            scale = 1f,
            velocity = 3f,
            skillSprite = weaponSprites[7],
            skillName = "SEVER",
            skilLDescription = "The SEVER skill is a high damaging skill that offers some base pierce. In " +
            "addition to this, it is fairly wide. This means that a barrage of this skill can cover " +
            "more area than other skills. However, this skill's cast time is slightly higher than " +
            "most. Using this skill with enemies who are clumped up will maximize its effectiveness.",
            duration = .65f,
            unlocked = 1,
            baseKnockback = 3.5f,
            pierce = 2,
            projectiles = 1,
            windup = 0f
        };

        spark = new Skill
        {
            damageChart = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8 },
            damagePrices = new List<int> { 50, 100, 200, 400, 800, 1600, 3200 },
            speedChart = new List<float> { 0f, .15f, .3f, .45f, .6f, .75f },
            speedPrices = new List<int> { 50, 100, 200, 400, 800 },
            sizeChart = new List<int> { 1, 2, 3 },
            sizePrices = new List<int> { 400, 800 },
            projectilesChart = new List<int> { 3, 4, 5, 6, 7 },
            projectilesPrices = new List<int> { 200, 400, 800, 1600 },
            velocityChart = new List<float> { 0f, .20f, .4f, .6f, .8f, 1.0f },
            velocityPrices = new List<int> { 50, 100, 200, 400, 800 },
            specialChart = new List<float> { 0f, .1f, .2f, .3f, .4f, .5f },
            specialPrices = new List<int> { 100, 200, 400, 800, 1600 },
            damageLevel = 0,
            speedLevel = 0,
            sizeLevel = 0,
            projectilesLevel = 0,
            velocityLevel = 0,
            specialLevel = 0,
            castSpeed = .75f,
            scale = 1f,
            velocity = 4.5f,
            skillSprite = weaponSprites[8],
            skillName = "SPARK",
            skilLDescription = "What the SPARK skill lacks in damage, it makes up for with excellent speed. The cast " +
            "speed is very fast, but the velocity of this skill is on another level. To top it off, this skill has" +
            " 3 projetiles base (which can be increased to 7). This allows for nearly full screen coverage.",
            duration = .4f,
            unlocked = 1,
            baseKnockback = .5f,
            pierce = 1,
            projectiles = 3,
            windup = 0f
        };


        skills = new List<Skill> { scythe, fireball, minigun, shatter, iceSpike, vortex, lightning, sever, spark };
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += LoadState;


        // Other loads
        coinsGain = GetComponent<AudioSource>();
        creditsAnim = credits.GetComponent<Animator>();
        sfxSound = 1.0f;
        musicSound = 1.0f;
    }

    private void Start()
    {

        completedWave = 0;
        currentWave = 0;

        usedMusic = new HashSet<int>();

        for (int i = 0; i < screenTints.Length; i++)
        {
            screenTints[i].SetTrigger("hide");
        }
        for (int i = 0; i < screenTints.Length; i++)
        {
            screenTintsTriggered[i] = false;
        }

        charLevel = GetCurrentLevel();
        GetCurrentSkill();
        sMenu.SkillMenuCall();
    }


    private void Update()
    {
        //if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Return))
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!maximized) 
            {
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                Screen.SetResolution(1920, 1080, true);
                maximized = true;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Screen.SetResolution(960, 540, false);
                maximized = false;
            }
        }

        // Quick SKILL change
        if (!MainMenu.inMainMenu)
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                AddSkillIndex();
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                SubtractSkillIndex();
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
        }

        // Quick SKILL change
        if (!MainMenu.inMainMenu)
        {
            int currentSpecial = specialManager.currentSkillindex;
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentSpecial != 0)
            {
                ReleaseScreenTint(currentSpecial + 1);
                specialManager.SetSkillIndex(0);
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentSpecial != 1)
            {
                ReleaseScreenTint(currentSpecial + 1);
                specialManager.SetSkillIndex(1);
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && currentSpecial != 2)
            {
                ReleaseScreenTint(currentSpecial + 1);
                specialManager.SetSkillIndex(2);
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && currentSpecial != 3)
            {
                ReleaseScreenTint(currentSpecial + 1);
                specialManager.SetSkillIndex(3);
                hud.UpdateHUD();
                audioSources[6].PlayOneShot(audioClips[6], sfxSound);
            }
        }

        // Music and Such
        if (MainMenu.inMainMenu)
        {
            audioSources[3].Stop();
            audioSources[4].Stop();
            if (!audioSources[2].isPlaying)
            {
                audioSources[2].PlayOneShot(audioClips[2], 1f);

            }
        }
        else
        {
            audioSources[2].Stop();
            if (!spawnerManager.waveOver && ((currentWave % 10f) == 0))
            {
                audioSources[3].Stop();
                if (!audioSources[4].isPlaying)
                {
                    audioSources[4].PlayOneShot(audioClips[4], 1f);
                    Debug.Log(audioSources[4].isPlaying);
                }
            }
            else
            {
                audioSources[4].Stop();
                DoMusic();
            }
        }
    }

    public void ShowScreenTint(int screen)
    {
        screenTints[screen].SetTrigger("show");
    }

    public void OneTimeScreenTint(int screen)
    {
        screenTints[screen].SetTrigger("show");
        screenTints[screen].SetTrigger("hide");
    }

    public void ReleaseScreenTint(int screen)
    {
        screenTints[screen].ResetTrigger("show");
        screenTints[screen].SetTrigger("hide");
    }

    public int GetIndex()
    {
        int t = Random.Range(0, musicFiles.Length);
        if (usedMusic.Contains(t))
        {
            GetIndex();
        }
        else
        {
            return t;
        }
        return -1;

    }

    public void DoMusic()
    {
        // Find new track index
        if(currentMusicIndex < 0)
        {
            if (usedMusic.Count == musicFiles.Length)
            {
                usedMusic.Clear();
            }
            currentMusicIndex = GetIndex();
            if (currentMusicIndex == -1)
            {
                return;
            }
        }
        audioSources[2].Stop();
        if (!audioSources[3].isPlaying)
        {
            Debug.Log("Current Song Index: " + currentMusicIndex);
            audioSources[3].PlayOneShot(musicFiles[currentMusicIndex], 1f);
            usedMusic.Add(currentMusicIndex);
            currentMusicIndex = -1;
        }
        //if (!audioSources[4].isPlaying)
        //{
        //    audioSources[4].PlayOneShot(audioClips[4], 1f);
        //}
    }

    public void OpenCredits()
    {
        creditsAnim.SetTrigger("show");
        GameManager.instance.PlayAudio(6);
    }

    public void CloseCredits()
    {
        creditsAnim.SetTrigger("hide");
        GameManager.instance.PlayAudio(6);
    }

    public void OpenSettings()
    {
        SettingsMenu.inSettings = true;
        GameManager.instance.PlayAudio(6);
        settingsMenu.anim.SetTrigger("show");
    }

    // ~~~~~~~~~~ Skill Implementation ~~~~~~~~~~

    // Get Curr Skill 
    public Skill GetCurrentSkill()
    {
        UpdateCurrentSkill();
        return currentSkill;
    }

    public void SetCurrentSkill(int x)
    {
        currentSkillIndex = x;
        UpdateCurrentSkill();

    }

    public void UpdateCurrentSkill()
    {
        currentSkill = skills[currentSkillIndex];
        sMenu.UpdateSkillMenu();
    }

    public void PlayAudio(int index)
    {
        if (!audioSources[index].isPlaying)
        {
            audioSources[index].PlayOneShot(audioClips[index], sfxSound);

        }
    }

    // Upgrade Skill
    public bool TryUpgradeSkill(List<int> skillType, int skillLevel)
    {
        // Is the weapon max level?
        if (skillType.Count <= skillLevel)
        {
            return false;
        }

        if (coins >= skillType[skillLevel])
        {
            coins -= skillType[skillLevel];
            //madeUpgrades[weapon.weaponLevel] = weaponPrices[weapon.weaponLevel];

            return true;
        }

        return false;
    }

    // Add/Subtract 1 to Index of currSkill
    public void AddSkillIndex()
    {
        if(currentSkillIndex < skills.Count - 1)
        {
            currentSkillIndex++;
            if (!skills[currentSkillIndex].IsUnlocked())
                AddSkillIndex();
        } 
        else
        {
            currentSkillIndex = 0;
            if (!skills[currentSkillIndex].IsUnlocked())
                AddSkillIndex();
        }
        UpdateCurrentSkill();
    }

    public void SubtractSkillIndex()
    {
        if (currentSkillIndex > 0)
        {
            currentSkillIndex--;
            if (!skills[currentSkillIndex].IsUnlocked())
                AddSkillIndex();
        }
        else
        {
            currentSkillIndex = skills.Count - 1;
            if (!skills[currentSkillIndex].IsUnlocked())
                AddSkillIndex();
        }
        UpdateCurrentSkill();
    }


    public void ResetSkill(int skillIndex)
    {
        int refundedCoins = 0;
        for (int i = 0; i < skills[currentSkillIndex].damageLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].damagePrices[i];
        }
        for (int i = 0; i < skills[currentSkillIndex].speedLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].speedPrices[i];
        }
        for (int i = 0; i < skills[currentSkillIndex].sizeLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].sizePrices[i];
        }
        for (int i = 0; i < skills[currentSkillIndex].projectilesLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].projectilesPrices[i];
        }
        for (int i = 0; i < skills[currentSkillIndex].velocityLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].velocityPrices[i];
        }
        for (int i = 0; i < skills[currentSkillIndex].specialLevel; i++)
        {
            refundedCoins += skills[currentSkillIndex].specialPrices[i];
        }
        skills[currentSkillIndex].SetDamageLevel(0);
        skills[currentSkillIndex].SetSpeedLevel(0);
        skills[currentSkillIndex].SetSizeLevel(0);
        skills[currentSkillIndex].SetProjectilesLevel(0);
        skills[currentSkillIndex].SetVelocityLevel(0);
        skills[currentSkillIndex].SetSpecialLevel(0);
        coins += refundedCoins;
        sMenu.UpdateSkillMenu();
    }

    // Floating text
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    // Sound for coins
    public void CoinsSound()
    {
        coinsGain.PlayOneShot(coinsGainSound, sfxSound);
    }

    // Upgrade Weapon
/*
    public bool TryUpgradeWeapon()
    {
         // Is the weapon max level?
         if (weaponPrices.Count <= weapon.weaponLevel)
        {
            return false;
        }

         if (coins >= weaponPrices[weapon.weaponLevel])
        {
            coins -= weaponPrices[weapon.weaponLevel];
            madeUpgrades[weapon.weaponLevel] = weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();

            return true;
        }

        return false;
    }*/


    // Level
    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while (experience >= add)
        {
            add += xpTable[r];
            r++;

            // Max Level
            if (r == xpTable.Count)
                return r;
        }

        return r;
    }

    public int GetXPToLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += xpTable[r];
            r++;
        }

        return xp;
    }

    public void GrantXP(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += xp;
        if (currLevel < GetCurrentLevel())
            LevelUp();
    }

    public void UpdatePitch()
    {
        //audioSources[3].pitch = .8f + (float)(.01 * completedWave);
        audioSources[3].pitch = 1f;
    }

    public void LevelUp()
    {
        Debug.Log("PLAYER LEVELED UP");
        player.OnLevelUp();
    }

    // Save State
    /*
     * INT preferedSkin
     * INT coins
     * INT experience
     * INT weaponLevel
     */

    public void SaveVolume()
    {
        audioSources[2].volume = musicSound * .25f;
        audioSources[3].volume = musicSound * .25f;
        audioSources[4].volume = musicSound * .25f;
        PlayerPrefs.SetFloat("SFX", sfxSound);
        PlayerPrefs.SetFloat("MUSIC", musicSound);
        Debug.Log("SETTINGS SAVED");
    }

    public void SaveState()
    {

        string s = "";
        s += preferredSkin.ToString() + "|";
        s += coins.ToString() + "|";
        s += experience.ToString() + "|";
        s += currentSkillIndex.ToString() + "|";
        s += completedWave.ToString() + "|";
        for (int i = 0; i < skills.Count; i++)
        {
            s += skills[i].GetDamageLevel().ToString() + "|";
            s += skills[i].GetSpeedLevel().ToString() + "|";
            s += skills[i].GetSizeLevel().ToString() + "|";
            s += skills[i].GetProjectilesLevel().ToString() + "|";
            s += skills[i].GetVelocityLevel().ToString() + "|";
            s += skills[i].GetSpecialLevel().ToString() + "|";
            s += skills[i].unlocked.ToString() + "|";
        }
        s += mapSelect.currentIndex.ToString() + "|";
        s += specialManager.currentSkillindex.ToString() + "|";
        s += player.maxMana.ToString() + "|";
        s += player.maxHitpoint.ToString() + "|";
        s += "999";

        player.hitpoint = player.maxHitpoint;
        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("GAME SAVED WITH: " + s);
        PlayAudio(5);
    }

    public void ResetInfo()
    {
        // Change player skin
        preferredSkin = 0;
        player.SwapSprite(preferredSkin);
        // Coins
        coins = 50;
        // XP
        experience = 0;
        charLevel = 1; // Gets numeric value for level
                       //if (charLevel != 1)
                       //     player.SetLevel(charLevel); // Adjust health states
                       // Skills
        player.maxHitpoint = 100;
        player.hitpoint = 100;
        player.maxMana = 20;
        player.manaAmount = 20;
        currentSkillIndex = 1;
        specialManager.currentSkillindex = 0;
        completedWave = 0;
        currentWave = 0;
        SetCurrentSkill(currentSkillIndex);
        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SetDamageLevel(0);
            skills[i].SetSpeedLevel(0);
            skills[i].SetSizeLevel(0);
            skills[i].SetProjectilesLevel(0);
            skills[i].SetVelocityLevel(0);
            skills[i].SetSpecialLevel(0);
            skills[i].unlocked = (skills[i].IsUnlocked() ? 1 : 0);

        }
        int d = 1;
        if (d == 1)
        {
            Debug.Log("LOADED FRESH INSTANCE SUCCESSFULLY");
        }
        else
        {
            Debug.Log("FAILED LOADING");
        }

        currentWave = completedWave;

        currentSpawnPoint = GameObject.Find("SpawnPoint").transform.position;
        player.transform.position = currentSpawnPoint;
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (PlayerPrefs.HasKey("SFX") && PlayerPrefs.HasKey("MUSIC"))
        {
            sfxSound = PlayerPrefs.GetFloat("SFX");
            musicSound = PlayerPrefs.GetFloat("MUSIC");
            Debug.Log("SETTINGS LOADED");
            settingsMenu.UpdateSoundBars();
            SaveVolume();
        }

        if (!PlayerPrefs.HasKey("SaveState"))
        {
            ResetInfo();
            return;
        }

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Change player skin
        preferredSkin = int.Parse(data[0]);
        player.SwapSprite(preferredSkin);
        // Coins
        coins = int.Parse(data[1]);
        // XP
        experience = int.Parse(data[2]);
        charLevel = GetCurrentLevel(); // Gets numeric value for level


        // Skills
        player.hitpoint = player.maxHitpoint;
        currentSkillIndex = int.Parse(data[3]);
        completedWave = int.Parse(data[4]);
        currentWave = completedWave;

        SetCurrentSkill(currentSkillIndex);

        for (int i = 0; i < skills.Count; i++)
        {
            skills[i].SetDamageLevel(int.Parse(data[5 + (i * 7)]));
            skills[i].SetSpeedLevel(int.Parse(data[6 + (i * 7)]));
            skills[i].SetSizeLevel(int.Parse(data[7 + (i * 7)]));
            skills[i].SetProjectilesLevel(int.Parse(data[8 + (i * 7)]));
            skills[i].SetVelocityLevel(int.Parse(data[9 + (i * 7)]));
            skills[i].SetSpecialLevel(int.Parse(data[10 + (i * 7)]));
            skills[i].unlocked = int.Parse(data[11 + (i * 7)]);
           
        }
        mapSelect.currentIndex = int.Parse(data[data.Length - 5]);
        specialManager.currentSkillindex = int.Parse(data[data.Length - 4]);
        player.maxMana = int.Parse(data[data.Length - 3]);
        player.maxHitpoint = int.Parse(data[data.Length - 2]);
        int done = int.Parse(data[data.Length-1]);
        if(done == 999)
        {
            Debug.Log("LOADED SUCCESSFULLY");
        } else
        {
            Debug.Log("FAILED LOADING");
        }
        
        
        // Table for Weapon
        pause.ResetTable();
        player.manaAmount = player.maxMana;
        player.hitpoint = player.maxHitpoint;
        currentWave = completedWave;
        currentSpawnPoint = GameObject.Find("SpawnPoint").transform.position;
        player.transform.position = currentSpawnPoint;
    }

    public void LoadGM(string scene)
    {
        //SceneManager.sceneLoaded -= LoadState;
        loadingBarText.text = "0%";
        loadingBar.localScale = new Vector3(0, 1, 1);
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene));

        StartCoroutine(GetSceneLoadProgress());

        SceneManager.sceneLoaded += LoadState;
        hud.healthBar.localScale = new Vector3(1, 1, 1);
        hud.healthText.text = player.maxHitpoint.ToString() + " / " + player.maxHitpoint.ToString();
        player.RefreshHealthBar();
        hud.UpdateHUD();
        sMenu.UpdateSkillMenu();
        hud.HealTaken(player.maxHitpoint);
        SaveState();
    }

    public void LoadGMInt(int scene)
    {
        //SceneManager.sceneLoaded -= LoadState;
        loadingBarText.text = "0%";
        loadingBar.localScale = new Vector3(0, 1, 1);
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene));

        StartCoroutine(GetSceneLoadProgress());

        SceneManager.sceneLoaded += LoadState;
        hud.healthBar.localScale = new Vector3(1, 1, 1);
        hud.healthText.text = player.maxHitpoint.ToString() + " / " + player.maxHitpoint.ToString();
        player.RefreshHealthBar();
        hud.UpdateHUD();
        sMenu.UpdateSkillMenu();
        hud.HealTaken(player.maxHitpoint);
    }

    public void LoadGMFresh(string scene)
    {
        PlayerPrefs.DeleteKey("SaveState");
        loadingBarText.text = "0%";
        loadingBar.localScale = new Vector3(0, 1, 1);
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene));

        StartCoroutine(GetSceneLoadProgress());

        SceneManager.sceneLoaded += LoadState;
        hud.healthBar.localScale = new Vector3(1, 1, 1);
        hud.healthText.text = player.maxHitpoint.ToString() + " / " + player.maxHitpoint.ToString();
        mapSelect.UpdateMapSelect();
        ResetInfo();
        player.RefreshHealthBar();
        player.alive = true;
        hud.UpdateHUD();
        sMenu.UpdateSkillMenu();
        hud.HealTaken(player.maxHitpoint);
    }

    public void LoadGMFreshInt(int scene)
    {
        PlayerPrefs.DeleteKey("SaveState");
        SceneManager.sceneLoaded += LoadState;
        loadingBarText.text = "0%";
        loadingBar.localScale = new Vector3(0, 1, 1);
        loadingScreen.SetActive(true);
        scenesLoading.Add(SceneManager.LoadSceneAsync(scene));

        StartCoroutine(GetSceneLoadProgress());

        hud.healthBar.localScale = new Vector3(1, 1, 1);
        hud.healthText.text = player.maxHitpoint.ToString() + " / " + player.maxHitpoint.ToString();
        mapSelect.UpdateMapSelect();
        ResetInfo();
        player.RefreshHealthBar();
        player.alive = true;
        hud.UpdateHUD();
        sMenu.UpdateSkillMenu();
        hud.HealTaken(player.maxHitpoint);

    }

    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < scenesLoading.Count; i++)
        {
            while(!scenesLoading[i].isDone)
            {
                totalSceneProgress = 0;
                foreach(AsyncOperation operation in scenesLoading)
                {
                    totalSceneProgress += operation.progress;
                }

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count);

                loadingBarText.text = (Mathf.RoundToInt(totalSceneProgress * 100f)).ToString("") + "%";
                loadingBar.localScale = new Vector3(totalSceneProgress, 1, 1);

                yield return null;
            }
            scenesLoading.Remove(scenesLoading[i]);

            loadingScreen.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("QUITTING GAME");
        Application.Quit();
    }
}
