using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController
{
    readonly string MeteorPath = $"Weapon/MageSkills/Meteor 1";
    public IEnumerator Co_ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        var meteor = WeaponSpawner.Spawn(MeteorPath, spawnPos).GetComponent<Multi_Meteor>();

        Action<Multi_Enemy> hitAction = (_) => HitAction(_, hitDamage, stunTime);
        Vector3 tempPos = target.transform.position;
        yield return new WaitForSeconds(1f);

        if (target.IsDead)
            meteor.Shot(null, tempPos, hitAction);
        else
            meteor.Shot(target, target.transform.position, hitAction);
    }

    void HitAction(Multi_Enemy target, int hitDamage, float stunTime)
    {
        target.OnDamage(hitDamage);
        target.OnStun_RPC(100, stunTime);
    }
}
