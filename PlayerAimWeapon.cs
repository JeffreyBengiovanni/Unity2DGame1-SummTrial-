using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    public event EventHandler<OnThrowEventArgs> OnThrow;

    public class OnThrowEventArgs : EventArgs
    {
        public Vector3 weaponEndPointPosition;
        public Vector3 throwPosition;
    }

    public static float aimRotationZ;
    public static Transform aimTransform;
    private Transform weaponEndPointTransform;
    public bool cooldownDone, canThrow;
    private SpriteRenderer spriteRenderer0, spriteRenderer1, spriteRenderer2, spriteRenderer3;
    private static PlayerAimWeapon objectInstance;
    private float windup, lastWindup;
    private bool throwing = false;

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

    private void Start()
    {
        aimTransform = transform.Find("Aim");
        weaponEndPointTransform = aimTransform.Find("WeaponEndPointPosition");
        
        spriteRenderer0 = transform.Find("PlayerSprite").Find("Sprite0").GetComponent<SpriteRenderer>();
        spriteRenderer1 = transform.Find("PlayerSprite").Find("Sprite1").GetComponent<SpriteRenderer>();
        spriteRenderer2 = transform.Find("PlayerSprite").Find("Sprite2").GetComponent<SpriteRenderer>();
        spriteRenderer3 = transform.Find("PlayerSprite").Find("Sprite3").GetComponent<SpriteRenderer>();

        cooldownDone = true;
    }

    private void Update()
    {
        windup = GameManager.instance.currentSkill.GetWindup();
        if (!MainMenu.inMainMenu && !GameManager.instance.loadingScreen.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0))
            {
                throwing = true;
                lastWindup = Time.time;
            }
            if (throwing)
            {
                if (Time.time - lastWindup >= windup || windup < .1f)
                {
                    canThrow = true;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                lastWindup = Time.time;
                canThrow = false;
                throwing = false;
            }

            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
            {
                if (GameManager.instance.player.alive)
                {
                    HandleAiming();
                    HandleThrowing();
                }
            }
        }
        else
        {
            canThrow = false;
            cooldownDone = true;
            throwing = false;
            StopAllCoroutines();
        }
    }

    private void HandleAiming()
    {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTransform.eulerAngles = new Vector3(0, 0, angle);
        aimRotationZ = angle;

        Vector3 a = Vector3.one;
        if (angle > 90 || angle < -90)
        {
            a.y = -1f;
        } 
        else
        {
            a.y = +1f;
        }

        // Right
        if (angle < 45 && angle > -45)
        {
            if(GameManager.instance.player.moving)
            {
                spriteRenderer0.sprite = GameManager.instance.playerMovingSprites[12];
                spriteRenderer1.sprite = GameManager.instance.playerMovingSprites[13];
                spriteRenderer2.sprite = GameManager.instance.playerMovingSprites[14];
                spriteRenderer3.sprite = GameManager.instance.playerMovingSprites[15];
            }
            else
            {
                spriteRenderer0.sprite = GameManager.instance.playerSprites[12];
                spriteRenderer1.sprite = GameManager.instance.playerSprites[13];
                spriteRenderer2.sprite = GameManager.instance.playerSprites[14];
                spriteRenderer3.sprite = GameManager.instance.playerSprites[15];
            }

        }
        // Left
        if (angle < -135 || angle > 135)
        {
            if (GameManager.instance.player.moving)
            {
                spriteRenderer0.sprite = GameManager.instance.playerMovingSprites[8];
                spriteRenderer1.sprite = GameManager.instance.playerMovingSprites[9];
                spriteRenderer2.sprite = GameManager.instance.playerMovingSprites[10];
                spriteRenderer3.sprite = GameManager.instance.playerMovingSprites[11];
            }
            else
            {
                spriteRenderer0.sprite = GameManager.instance.playerSprites[8];
                spriteRenderer1.sprite = GameManager.instance.playerSprites[9];
                spriteRenderer2.sprite = GameManager.instance.playerSprites[10];
                spriteRenderer3.sprite = GameManager.instance.playerSprites[11];
            }
        }
        // Down
        if (angle > 45 && angle < 135)
        {
            if (GameManager.instance.player.moving)
            {
                spriteRenderer0.sprite = GameManager.instance.playerMovingSprites[4];
                spriteRenderer1.sprite = GameManager.instance.playerMovingSprites[5];
                spriteRenderer2.sprite = GameManager.instance.playerMovingSprites[6];
                spriteRenderer3.sprite = GameManager.instance.playerMovingSprites[7];
            }
            else
            {
                spriteRenderer0.sprite = GameManager.instance.playerSprites[4];
                spriteRenderer1.sprite = GameManager.instance.playerSprites[5];
                spriteRenderer2.sprite = GameManager.instance.playerSprites[6];
                spriteRenderer3.sprite = GameManager.instance.playerSprites[7];
            }
        }
        // Up
        if (angle > -135 && angle < -45)
            {
                if (GameManager.instance.player.moving)
                {
                spriteRenderer0.sprite = GameManager.instance.playerMovingSprites[0];
                spriteRenderer1.sprite = GameManager.instance.playerMovingSprites[1];
                spriteRenderer2.sprite = GameManager.instance.playerMovingSprites[2];
                spriteRenderer3.sprite = GameManager.instance.playerMovingSprites[3];
            }
            else
            {
                spriteRenderer0.sprite = GameManager.instance.playerSprites[0];
                spriteRenderer1.sprite = GameManager.instance.playerSprites[1];
                spriteRenderer2.sprite = GameManager.instance.playerSprites[2];
                spriteRenderer3.sprite = GameManager.instance.playerSprites[3];
            }

        }
        aimTransform.localScale = a;
    }

    private void HandleThrowing()
    {
        Vector3 mousePosition = GetMouseWorldPosition();
        if (cooldownDone) {
            if (canThrow)
            {
                GameManager.instance.hud.RollCooldown();
                cooldownDone = false;
                StartCoroutine(Cooldown());
                if(GameManager.instance.player.isBerserk && !GameManager.instance.spawnerManager.waveOver)
                {
                    GameManager.instance.specialManager.ManaWorks();
                    GameManager.instance.specialManager.interacted = true;
                    GameManager.instance.player.manaAmount -= 3f;
                }
                GameManager.instance.hud.UpdateManaBar();
                OnThrow?.Invoke(this, new OnThrowEventArgs
                {
                    weaponEndPointPosition = weaponEndPointTransform.position,
                    throwPosition = mousePosition
                });
            }
        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(GameManager.instance.currentSkill.GetSpeed());
        cooldownDone = true;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera)
    {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }

}
