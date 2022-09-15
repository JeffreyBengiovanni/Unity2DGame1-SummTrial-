using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charm : ThrowingWeapon
{
    public override void Collided(Collider2D collider)
    {
        if (collider.tag == "Fighter")
        {
            collider.GetComponentInParent<Enemy>().dmgInc = 1.15f;
            collider.GetComponentInParent<Enemy>().incDmg = true;

        }
    }
}
