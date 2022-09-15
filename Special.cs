using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Special : MonoBehaviour
{
    // ~~~~~~~ TYPES ~~~~~~~~
    // 0 is time slow
    // 1 is heal
    // 2 is invincible
    // 3 is berserk
    public int maxSkillIndex = 3;
    public int currentSkillindex = 0;
    public List<string> names;
    public static Special objectInstance;
    public float currentTime;
    public float cooldown;
    public bool changedSkill;
    public bool canUseMana = true;
    public bool interacted = false;

    public void Awake()
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

    public void Start()
    {
        cooldown = 2f;
    }

    public void FixedUpdate()
    {
        Cooldown();
    }

    public void Cooldown()
    {
        if (currentTime < 0)
        {
            GameManager.instance.player.canRegen = true;
        } 
        else
        {
            currentTime = currentTime - Time.deltaTime;
        }
    }

    public void ManaWorks()
    {
        interacted = false;
        currentTime = cooldown;
        GameManager.instance.player.canRegen = false;
    }

    public virtual void DoSkill()
    {
        // TIME SLOW
        if (currentSkillindex == 0)
        {
            if (Input.GetMouseButton(1) && !changedSkill && canUseMana)
            {
                if (!GameManager.instance.spawnerManager.waveOver && interacted)
                    ManaWorks();
                if (GameManager.instance.player.manaAmount > 0)
                {
                    interacted = true;
                    GameManager.instance.ShowScreenTint(1);
                    GameManager.instance.PlayAudio(8);
                    Time.timeScale = .75f;
                    if (!GameManager.instance.spawnerManager.waveOver)
                    {
                        GameManager.instance.player.manaAmount -= .07f;
                        GameManager.instance.hud.UpdateManaBar();
                    }
                }
                else
                {
                    interacted = false;
                    canUseMana = false;
                    GameManager.instance.ReleaseScreenTint(1);
                }
            }
            else
            {
                if (!Input.GetMouseButton(1))
                {
                    changedSkill = false;
                    canUseMana = true;
                }
                interacted = false;
                GameManager.instance.ReleaseScreenTint(1);
                Time.timeScale = 1f;
            }
        }
        else
        {
            Time.timeScale = 1f;
        }

        // HEAL
        if (currentSkillindex == 1)
        {
            if (Input.GetMouseButton(1) &&!changedSkill && GameManager.instance.player.hitpoint != GameManager.instance.player.maxHitpoint && canUseMana)
            {
                if (!GameManager.instance.spawnerManager.waveOver && interacted)
                    ManaWorks();

                if (GameManager.instance.player.manaAmount > 0)
                {
                    interacted = true;
                    GameManager.instance.hud.HealTaken(1);
                    GameManager.instance.ShowScreenTint(2);
                    if (!GameManager.instance.spawnerManager.waveOver)
                    {
                        GameManager.instance.player.manaAmount -= .5f;
                        GameManager.instance.hud.UpdateManaBar();
                    }
                }
                else
                {
                    interacted = false;
                    canUseMana = false;
                    GameManager.instance.ReleaseScreenTint(2);
                }
            }
            else
            {
                if (!Input.GetMouseButton(1))
                {
                    changedSkill = false;
                    canUseMana = true;
                }
                interacted = false;
                GameManager.instance.ReleaseScreenTint(2);
            }
        }

        // INVINCIBILITY
        if (currentSkillindex == 2)
        {
            if (GameManager.instance.player.manaAmount >= 10f)
                canUseMana = true;
            if (Input.GetMouseButton(1) && !changedSkill && canUseMana)
            {
                if (GameManager.instance.player.manaAmount >= 9.5f)
                {
                    if (!GameManager.instance.spawnerManager.waveOver && interacted)
                        ManaWorks();
                    GameManager.instance.ShowScreenTint(3);
                    GameManager.instance.player.isInvincible = true;
                    if (!GameManager.instance.spawnerManager.waveOver)
                    {
                        //GameManager.instance.player.manaAmount -= .1f;
                        GameManager.instance.hud.UpdateManaBar();
                    }
                }
                else
                {
                    interacted = false;
                    canUseMana = false;
                    GameManager.instance.player.isInvincible = false;
                    GameManager.instance.ReleaseScreenTint(3);
                }
            }
            else
            {
                if (!Input.GetMouseButton(1))
                {
                    changedSkill = false;
                }
                interacted = false;
                GameManager.instance.ReleaseScreenTint(3);
                GameManager.instance.player.isInvincible = false;
            }
        }
        else
        {
            GameManager.instance.ReleaseScreenTint(3);

            GameManager.instance.player.isInvincible = false;
        }


        // BERSERK
        if (currentSkillindex == 3)
        {
            if (GameManager.instance.player.manaAmount >= 3f)
                canUseMana = true;
            if (Input.GetMouseButton(1) && !changedSkill && canUseMana)
            {
                if (GameManager.instance.player.manaAmount >= 2.5f)
                {
                    if (!GameManager.instance.spawnerManager.waveOver && interacted)
                        ManaWorks();
                    GameManager.instance.ShowScreenTint(4);
                    GameManager.instance.player.isBerserk = true;
                    if (!GameManager.instance.spawnerManager.waveOver)
                    {
                        //GameManager.instance.player.manaAmount -= .07f;
                        GameManager.instance.hud.UpdateManaBar();
                    }
                }
                else
                {
                    interacted = false;
                    canUseMana = false;
                    GameManager.instance.player.isBerserk = false;
                    GameManager.instance.ReleaseScreenTint(4);
                }
            }
            else
            {
                if (!Input.GetMouseButton(1))
                {
                    changedSkill = false;
                }
                interacted = false;
                GameManager.instance.ReleaseScreenTint(4);
                GameManager.instance.player.isBerserk = false;
            }
        }
        else
        {
            GameManager.instance.ReleaseScreenTint(4);

            GameManager.instance.player.isBerserk = false;
        }


        // Edge case for mana
        if (GameManager.instance.player.manaAmount < 0)
        {
            GameManager.instance.player.manaAmount = 0;
        }
    }

    public void SetSkillIndex(int num)
    {
        currentSkillindex = num;
        GameManager.instance.sMenu.UpdateSkillMenu();

    }

    public void IncSkillIndex()
    {
        currentSkillindex++;
        changedSkill = true;
        GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
        if (currentSkillindex > maxSkillIndex)
        {
            currentSkillindex = 0;
        }
        GameManager.instance.sMenu.UpdateSkillMenu();
    }

    public void DecSkillIndex()
    {
        currentSkillindex--;
        changedSkill = true;
        GameManager.instance.audioSources[6].PlayOneShot(GameManager.instance.audioClips[6], GameManager.instance.sfxSound);
        if (currentSkillindex < 0)
        {
            currentSkillindex = maxSkillIndex;
        }
        GameManager.instance.sMenu.UpdateSkillMenu();
    }
}
