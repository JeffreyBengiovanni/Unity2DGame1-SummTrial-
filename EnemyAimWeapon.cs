using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimWeapon : MonoBehaviour
{
    public event EventHandler<OnThrowEventArgs> OnThrow;

    public class OnThrowEventArgs : EventArgs
    {
        public Vector3 weaponEndPointPosition;
        public Vector3 throwPosition;
        public Quaternion aimT;
    }

    public Vector3 predictedPosition;
    public static float aimRotationZ;
    public static Transform aimTransform;
    private Transform weaponEndPointTransform, playerTransform;
    private SpriteRenderer spriteRenderer0, spriteRenderer1, spriteRenderer2, spriteRenderer3;
    private bool cooldownDone;
    private Enemy enemyRef;
    private EnemyThrowable enemyThrowable;
    public float cooldownLow = 3f, cooldownHigh = 5f;


    private void Start()
    {
        aimTransform = this.transform.Find("Aim").transform;
        enemyThrowable = this.transform.Find("Aim").Find("WeaponTesting").GetComponent<EnemyWeaponTesting>().skillThrows[0];
        weaponEndPointTransform = aimTransform.Find("WeaponEndPointPosition");
        enemyRef = transform.GetComponent<Enemy>();
        spriteRenderer0 = this.transform.Find("EnemySprite").Find("Sprite0").GetComponent<SpriteRenderer>();
        spriteRenderer1 = this.transform.Find("EnemySprite").Find("Sprite1").GetComponent<SpriteRenderer>();
        spriteRenderer2 = this.transform.Find("EnemySprite").Find("Sprite2").GetComponent<SpriteRenderer>();
        spriteRenderer3 = this.transform.Find("EnemySprite").Find("Sprite3").GetComponent<SpriteRenderer>();
        

        cooldownDone = true;
    }

    private void Update()
    {
        if (GameManager.instance.player.alive)
        {
            if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
            {
                HandleAiming();
                HandleThrowing();
            }
        }
    }


    private void HandleAiming()
    {
        playerTransform = GameManager.instance.player.transform;
        Vector3 aimDirection = (playerTransform.position - transform.position).normalized;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
//         Debug.Log("NUM " + (GameManager.instance.player.GetVelocity() * enemyThrowable.speedTotal));
//         Debug.Log("Aim: " + (playerTransform.position - transform.position).normalized);
//         Debug.Log("Aim Angle: " + Mathf.Atan2((playerTransform.position - transform.position).y, (playerTransform.position - transform.position).x) * Mathf.Rad2Deg);
//         Debug.Log("Corrected Aim: " + aimDirection);
//         Debug.Log("Corrected Aim Angle: " + angle);
//         Debug.Log("~~~~~~~~~~~");
        if (aimTransform != null)
        {
            aimTransform.eulerAngles = new Vector3(0, 0, angle);
            aimRotationZ = angle;
            //Debug.Log(angle);

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
                spriteRenderer0.sprite = enemyRef.enemySprites[12];
                spriteRenderer1.sprite = enemyRef.enemySprites[13];
                spriteRenderer2.sprite = enemyRef.enemySprites[14];
                spriteRenderer3.sprite = enemyRef.enemySprites[15];
            }
            // Left
            if (angle < -135 || angle > 135)
            {
                spriteRenderer0.sprite = enemyRef.enemySprites[8];
                spriteRenderer1.sprite = enemyRef.enemySprites[9];
                spriteRenderer2.sprite = enemyRef.enemySprites[10];
                spriteRenderer3.sprite = enemyRef.enemySprites[11];
            }
            // Down
            if (angle > 45 && angle < 135)
            {
                spriteRenderer0.sprite = enemyRef.enemySprites[4];
                spriteRenderer1.sprite = enemyRef.enemySprites[5];
                spriteRenderer2.sprite = enemyRef.enemySprites[6];
                spriteRenderer3.sprite = enemyRef.enemySprites[7];
            }
            // Up
            if (angle > -135 && angle < -45)
            {
                spriteRenderer0.sprite = enemyRef.enemySprites[0];
                spriteRenderer1.sprite = enemyRef.enemySprites[1];
                spriteRenderer2.sprite = enemyRef.enemySprites[2];
                spriteRenderer3.sprite = enemyRef.enemySprites[3];
            }
            aimTransform.localScale = a;
        } 
        else
        {
            aimTransform = this.transform.Find("Aim").transform;
        }
    }


    private void HandleThrowing()
    {
        if (cooldownDone && enemyRef.PlayerInRange() && enemyRef.alive) 
        {
            cooldownDone = false;
            StartCoroutine(Cooldown());
            OnThrow?.Invoke(this, new OnThrowEventArgs
            {
                weaponEndPointPosition = weaponEndPointTransform.position,
                throwPosition = transform.position,
                aimT = aimTransform.rotation
            });
        }
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range( (cooldownLow * (1 - (0.005f * GameManager.instance.completedWave))), (cooldownHigh * (1 - (0.005f * GameManager.instance.completedWave))) ));
        cooldownDone = true;
    }

}
