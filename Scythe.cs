using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scythe : ThrowingWeapon
{
    public HitEffect effect;
    public override void Collided(Collider2D collider)
    {
        Instantiate(effect, transform.position, transform.rotation);
    }
}
