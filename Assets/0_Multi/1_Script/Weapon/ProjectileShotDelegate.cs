using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProjectileData
{
    [SerializeField] string weaponPath;
    [SerializeField] Transform attacker;
    [SerializeField] Transform spawnTransform;

    public ProjectileData(string weaponPath, Transform attacker, Transform spawnPos)
    {
        this.weaponPath = weaponPath;
        this.attacker = attacker;
        this.spawnTransform = spawnPos;
    }

    public string WeaponPath => weaponPath;
    public Transform Attacker => attacker;
    public Transform SpawnTransform => spawnTransform;
    public Vector3 SpawnPos => spawnTransform.position;
}

public static class ProjectileShotDelegate
{
    static Multi_Projectile GetProjectile(ProjectileData data, Vector3 spawnPos)
        => Multi_SpawnManagers.Weapon.Spawn(data.WeaponPath, spawnPos).GetComponent<Multi_Projectile>();

    public static Multi_Projectile ShotProjectile(ProjectileData data, Vector3 dir, Action<Multi_Enemy> hitAction)
        => ShotProjectile(data, data.SpawnPos, dir, hitAction);

    public static Multi_Projectile ShotProjectile(ProjectileData data, Vector3 spawnPos, Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon = GetProjectile(data, spawnPos);
        UseWeapon.Shot(dir, hitAction);
        return UseWeapon;
    }

    public static void ShotProjectile(ProjectileData data, Transform target,  Action<Multi_Enemy> hitAction, float weightRate = 2f)
        => ShotProjectile(data, Get_ShootDirection(data.Attacker, target, weightRate), hitAction);

    // 원거리 무기 발사
    static Vector3 Get_ShootDirection(Transform attacker, Transform _target, float weightRate = 2f)
    {
        // 속도 가중치 설정(적보다 약간 앞을 쏨, 적군의 성 공격할 때는 의미 없음)
        if (_target != null)
        {
            Multi_Enemy enemy = _target.GetComponent<Multi_Enemy>();
            if (enemy != null)
            {
                Vector3 dir = _target.position - attacker.position;
                float enemyWeightDir = Mathf.Lerp(0, weightRate, Vector3.Distance(_target.position, attacker.position) * 2 / 100);
                dir += enemy.dir.normalized * (0.5f * enemy.Speed) * enemyWeightDir;
                return dir.normalized;
            }
            else return (_target.position - attacker.position).normalized;
        }
        else return attacker.forward.normalized;
    }
}
