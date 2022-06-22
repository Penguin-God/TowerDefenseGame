using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestUtility : MonoBehaviour
{
    [ContextMenu("Test UsedWeapon")]
    void TestUsedWeapon()
    {
        UsedWeapon(projectile.gameObject, transform.position, transform.forward, null);
    }

    [SerializeField] Multi_Projectile projectile;
    protected Multi_Projectile UsedWeapon(GameObject go, Vector3 weaponPos, Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon = Multi_SpawnManagers.Weapon.Spawn(go, new Vector3(weaponPos.x, 2f, weaponPos.z)).GetComponent<Multi_Projectile>();
        UseWeapon.Shot(dir, (Multi_Enemy enemy) => hitAction(enemy));

        return UseWeapon;
    }
}
