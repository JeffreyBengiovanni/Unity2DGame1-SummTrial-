using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleStar : EnemyThrowingWeapon
{
    public int dmg;
    protected override void Start()
    {
        base.Start();
        damage = Mathf.RoundToInt((dmg * (float)(1 + GameManager.instance.completedWave / 100f)));
        push = 2;
    }
}
