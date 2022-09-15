using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shatter : ThrowingWeapon
{
    public ThrowingWeapon shatter;
    private Quaternion quat1, quat2;
    private Animator anim;
    public HitEffect effect;

    protected override void Start()
    {
        base.Start();
        anim = transform.Find("SpriteRenderer").GetComponent<Animator>();
        quat1 = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z + .3f, transform.rotation.w);
        quat2 = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z - .3f, transform.rotation.w);

    }
    public override void Collided(Collider2D collider)
    {
        Instantiate(effect, transform.position, transform.rotation);
        ThrowingWeapon t1 = Instantiate(shatter, collider.transform.position, quat1);

        ThrowingWeapon t2 = Instantiate(shatter, collider.transform.position, quat2);


    }
}
