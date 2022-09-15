using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyThrowingWeapon : Collidable
{
    // Damage struct

    // Upgrade
    public int damage = 0;
    public int push = 0;
    public SpriteRenderer spriteRenderer;
    public Transform spriteTransform;

    // Pierce
    public HashSet<Collider2D> collidedWith;
    public int pierceAmount;

    // Swing
    private Animator anim;


    private void Awake()
    {
        spriteRenderer = transform.Find("SpriteRenderer").GetComponent<SpriteRenderer>();
        spriteTransform = transform.Find("SpriteRenderer");
    }

    protected override void Start()
    {
        base.Start();
        damage = (int)(GameManager.instance.enemyDamages[0] * (1 + GameManager.instance.completedWave / 100));
        push = 1;
        collidedWith = new HashSet<Collider2D>();
        pierceAmount = 1;
        if (EnemyAimWeapon.aimTransform)
        {
            if (EnemyAimWeapon.aimTransform.localScale.y > 0)
            {
                spriteTransform.localScale = new Vector3(1, 1, 0);
            }
            else
            {
                spriteTransform.localScale = new Vector3(1, -1, 0);

            }

            spriteTransform.eulerAngles = Vector3.forward * EnemyAimWeapon.aimRotationZ;
            //anim = GetComponent<Animator>();
        }
    }

    protected override void FixedUpdate()
    {
        if(GameManager.instance.spawnerManager.waveOver)
        {
            Destroy(gameObject);
        }
        if (!MainMenu.inMainMenu && Pause.gameActive && SkillPause.gameActive && DialoguePause.gameActive)
        {
            // Collision Work
            boxCollider.OverlapCollider(filter, hits);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                if (hits[i])
                {
                    OnCollide(hits[i]);
                }

                // The array is not cleaned up, so we clean it
                hits[i] = null;
            }

            // Collision Work for interactions
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i] == null)
                    continue;

                OnCollide(hits[i]);

                // The array is not cleaned up, so we clean it
                hits[i] = null;
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.tag == "AllBlocking")
        {
            OnCollide(coll);
            ToDestroy();
        }

        if (coll.tag == "Fighter")
        {
            if (coll.name != "Player")
                return;


            if (collidedWith.Contains(coll))
            {
                return;
            }
            else
            {
                // Create a new Damage object, send to fighter we hit
                Damage dmg = new Damage
                {
                    damageAmount = damage,
                    origin = transform.position,
                    pushForce = push
                };

                coll.SendMessage("RecieveDamage", dmg);
                collidedWith.Add(coll);
            }

            if (collidedWith.Count >= pierceAmount)
            {
                Destroy(gameObject);
            }
        }

    }
    public virtual void Collided(Collider2D collider)
    {
        Debug.Log("COLLIDED NOT IMPLEMENTED");
    }

    public virtual void ToDestroy()
    {
        Destroy(gameObject);
    }

}

