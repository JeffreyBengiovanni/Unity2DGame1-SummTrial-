using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : ThrowingWeapon
{

    public EnemyDamager explosion;

    public override void Collided(Collider2D collider)
    {
        Instantiate(explosion, collider.transform.position, transform.rotation);
    }
}
