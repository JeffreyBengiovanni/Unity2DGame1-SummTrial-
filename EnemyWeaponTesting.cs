using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponTesting : MonoBehaviour
{
    [SerializeField] private EnemyAimWeapon enemyAimWeapon;
    public List<EnemyThrowable> skillThrows;
    public int damage;
    private Transform aimTransform, parentTransform, weaponEndPointTransform;
    private Quaternion quat1, quat2, quat3, quat4;
    public Enemy enemyRef;
    public int projectilesCount;

    private void Start()
    {
        quat1 = new Quaternion(GameManager.instance.player.transform.Find("PlayerSprite").rotation.x,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.y,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.z + .4f,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.w);
        quat2 = new Quaternion(GameManager.instance.player.transform.Find("PlayerSprite").rotation.x,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.y,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.z - .4f,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.w);
        quat3 = new Quaternion(GameManager.instance.player.transform.Find("PlayerSprite").rotation.x,
                               GameManager.instance.player.transform.Find("PlayerSprite").rotation.y,
                               GameManager.instance.player.transform.Find("PlayerSprite").rotation.z + .8f,
                               GameManager.instance.player.transform.Find("PlayerSprite").rotation.w);
        quat4 = new Quaternion(GameManager.instance.player.transform.Find("PlayerSprite").rotation.x,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.y,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.z - .8f,
                                GameManager.instance.player.transform.Find("PlayerSprite").rotation.w);
        weaponEndPointTransform = transform.parent.Find("WeaponEndPointPosition");
        damage = enemyRef.damage;
        enemyAimWeapon.OnThrow += EnemyAimWeapon_OnThrow;
    }

    private void EnemyAimWeapon_OnThrow(object sender, EnemyAimWeapon.OnThrowEventArgs e)
    {
        if (Pause.gameActive && SkillPause.gameActive)
        {
            if (projectilesCount >= 1)
            {
                Instantiate(skillThrows[0], e.weaponEndPointPosition, Quaternion.identity);
            }
            if (projectilesCount >= 2)
            {
                Instantiate(skillThrows[0], e.weaponEndPointPosition, quat1);
            }
            if (projectilesCount >= 3)
            {
                Instantiate(skillThrows[0], e.weaponEndPointPosition, quat2);
            }
            if (projectilesCount >= 4)
            {
                Instantiate(skillThrows[0], e.weaponEndPointPosition, quat3);
            }
            if (projectilesCount >= 5)
            {
                Instantiate(skillThrows[0], e.weaponEndPointPosition, quat4);
            }
        }
    }



}
