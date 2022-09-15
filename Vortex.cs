using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : ThrowingWeapon
{

    public HitEffect effect;

    public override void Collided(Collider2D collider)
    {
        if(collider)
        {
            Instantiate(effect, collider.transform.position, collider.transform.rotation);
        }
        else
        {
            Instantiate(effect, transform.position, transform.rotation);
        }
        StartCoroutine(AllowHit(collider));
    }

    public IEnumerator AllowHit(Collider2D collider)
    {
        yield return new WaitForSeconds(GameManager.instance.GetCurrentSkill().GetSpeed()/3f);
        collidedWith.Remove(collider);
    }
}
