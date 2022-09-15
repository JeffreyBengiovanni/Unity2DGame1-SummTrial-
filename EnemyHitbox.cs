using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : Collidable
{
    // Damage
    private int damage = 0;
    public float pushForce;

    protected override void Start()
    {
        base.Start();
        damage = transform.GetComponentInParent<Enemy>().damage * 2;
        filter.SetLayerMask(LayerMask.GetMask("PlayerAndAlly"));
    }
    protected override void FixedUpdate()
    {
        if (!MainMenu.inMainMenu)
        {
            if (Pause.gameActive && SkillPause.gameActive)
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
    }

    protected override void OnCollide(Collider2D coll)
    {

        if (coll.tag == "Fighter" && coll.name == "Player")
        {
            // Create a new damage object, before sending it to the player
            Damage dmg = new Damage
            {
                damageAmount = damage,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("RecieveDamage", dmg);
        }
    }
}
