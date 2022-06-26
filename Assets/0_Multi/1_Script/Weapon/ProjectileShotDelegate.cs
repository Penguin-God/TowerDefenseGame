﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProjectileData
{
    [SerializeField] Multi_Projectile original;
    [SerializeField] Transform attacker;
    [SerializeField] Transform spawnTransform;

    public ProjectileData(Multi_Projectile original, Transform attacker, Transform spawnPos)
    {
        this.original = original;
        this.attacker = attacker;
        this.spawnTransform = spawnPos;
    }

    public Multi_Projectile Original => original;
    public Transform Attacker => attacker;
    public Transform SpawnTransform => spawnTransform;
    public Vector3 SpawnPos => spawnTransform.position;

    protected Multi_Projectile UsedWeapon(WeaponType weaponType, GameObject go, 
                                            Transform weaponPos, Vector3 dir, 
                                            int speed, Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon =
                Multi_SpawnManagers.Weapon.Spawn(go).GetComponent<Multi_Projectile>();

        UseWeapon.Shot(weaponPos.position, dir, speed, hitAction);
        return UseWeapon;
    }
}

public static class ProjectileShotDelegate
{
    static Multi_Projectile GetProjectile(ProjectileData data)
        => Multi_SpawnManagers.Weapon.Spawn(data.Original.gameObject, data.SpawnPos).GetComponent<Multi_Projectile>();

    public static Multi_Projectile ShotProjectile(ProjectileData data, Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon = GetProjectile(data);
        UseWeapon.Shot(dir, hitAction);
        return UseWeapon;
    }

    public static Multi_Projectile ShotProjectile(ProjectileData data, Transform target, float weightRate, Action<Multi_Enemy> hitAction)
        => ShotProjectile(data, Get_ShootDirection(data.Attacker, target, weightRate), hitAction);

    // 원거리 무기 발사
    static Vector3 Get_ShootDirection(Transform attacker, Transform _target, float weightRate)
    {
        // 속도 가중치 설정(적보다 약간 앞을 쏨, 적군의 성 공격할 때는 의미 없음)
        if (_target != null)
        {
            Multi_Enemy enemy = _target.GetComponent<Multi_Enemy>();
            if (enemy != null)
            {
                Vector3 dir = _target.position - attacker.position;
                float enemyWeightDir = Mathf.Lerp(0, weightRate, Vector3.Distance(_target.position, attacker.position) * 2 / 100);
                dir += enemy.dir.normalized * (0.5f * enemy.speed) * enemyWeightDir;
                return dir.normalized;
            }
            else return (_target.position - attacker.position).normalized;
        }
        else return attacker.forward.normalized;
    }
}
