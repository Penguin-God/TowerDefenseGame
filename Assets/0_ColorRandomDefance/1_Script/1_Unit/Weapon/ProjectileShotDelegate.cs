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

// 레거시
public static class ProjectileShotDelegate
{
    public static Multi_Projectile ShotProjectile(ProjectileData data, Vector3 dir, Action<Multi_Enemy> hitAction)
    {
        Multi_Projectile UseWeapon = Managers.Multi.Instantiater.PhotonInstantiate(data.WeaponPath).GetComponent<Multi_Projectile>(); 
        UseWeapon.Shot(dir, hitAction);
        return UseWeapon;
    }

    // 원거리 무기 발사
    static Vector3 Get_ShootPath(Transform attacker, Multi_Enemy target)
    {
        if (target == null) return attacker.forward.normalized;

        if (target.enemyType == EnemyType.Tower)
            return new ShotPathCalculator().CalculatePath_To_StaticTarget(attacker.position, target.transform.position);
        else
            return new ShotPathCalculator().CalculatePath_To_MoveTarget(attacker.position, target.transform.position, target.Speed, target.dir);
    }
}
