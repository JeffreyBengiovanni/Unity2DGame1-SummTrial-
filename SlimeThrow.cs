using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeThrow : EnemyThrowingWeapon
{
    protected override void Start()
    {
        base.Start();
        damage = Mathf.RoundToInt((GameManager.instance.enemyDamages[0] * (float)(1 + GameManager.instance.completedWave / 100f)));
        push = 1;
    }
}
