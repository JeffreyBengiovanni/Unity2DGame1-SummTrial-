using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    // Public fields
    public int hitpoint;
    public int maxHitpoint;
    public float pushRecoverySpeed = 0.1f;
    
    // Status
    public bool alive;

    // Immunity
    protected float immuneTime = 1f;
    protected float lastImmune;
    protected float immuneTimeDmg = 0;
    protected float lastImmuneDmg;

    // Push
    protected Vector3 pushDirection;
    protected Vector3 bounceDirectionX;
    protected Vector3 bounceDirectionY;
    protected Vector3 bounceForceX;
    protected Vector3 bounceForceY;


    // All fighters can RecieveDamage / Die
    protected virtual void RecieveDamage(Damage dmg)
    {
        if (alive)
        {
            if (Time.time - lastImmune > immuneTime)
            {
                lastImmune = Time.time;
                hitpoint -= dmg.damageAmount;
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.zero, 0.5f);

                if (hitpoint <= 0)
                {
                    hitpoint = 0;
                    Death();
                }
            }
        }
    }


    protected virtual void Death()
    {
        alive = false;
    }

}
