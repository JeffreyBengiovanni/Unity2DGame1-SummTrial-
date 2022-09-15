using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpike : ThrowingWeapon
{

    public HitEffect effect;

    public override void Collided(Collider2D collider)
    {
        Instantiate(effect, transform.position, transform.rotation);

        if (collider.tag == "Fighter")
        {
            collider.GetComponentInParent<Enemy>().slowModifier = .8f;
            collider.GetComponentInParent<Enemy>().isSlowed = true;
        }
    }

}
