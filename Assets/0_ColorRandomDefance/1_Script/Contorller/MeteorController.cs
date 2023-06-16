using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorController : MonoBehaviour
{
    readonly string MeteorPath = $"Weapon/MageSkills/Meteor 1";

    void HitAction(Multi_Enemy target, int hitDamage, float stunTime)
    {
        target.OnDamage(hitDamage);
        target.OnStun_RPC(100, stunTime);
    }

    public void ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        Action<Multi_Enemy> hitAction = (_) => HitAction(_, hitDamage, stunTime);
        StartCoroutine(Co_ShotMeteor(target, hitAction, spawnPos));
    }

    IEnumerator Co_ShotMeteor(Multi_Enemy target, Action<Multi_Enemy> hitAction, Vector3 spawnPos)
    {
        var meteor = WeaponSpawner.Spawn(MeteorPath, spawnPos).GetComponent<Multi_Meteor>();
        Vector3 tempPos = target.transform.position;
        yield return new WaitForSeconds(1f);

        if (target.IsDead)
            meteor.Shot(null, tempPos, hitAction);
        else
            meteor.Shot(target, target.transform.position, hitAction);
    }
}
