using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ProjectileThrowingUnit : MonoBehaviourPun
{
    ProjectileData projectileData;
    public void SetInfo(string weaponPath, Transform weaponThrowPoint) => projectileData = new ProjectileData(weaponPath, transform, weaponThrowPoint);
    [SerializeField] Transform _weaponThrowPoint;

    public Multi_Projectile FlatThrow(Transform target, Action<Multi_Enemy> onHit) => FlatThrow(projectileData.WeaponPath, target, onHit);
    public Multi_Projectile FlatThrow(string weaponPath, Transform target, Action<Multi_Enemy> onHit) => CallThrow(CreateProjectile(weaponPath, onHit), target);

    Multi_Projectile CreateProjectile(string weaponPath, Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(weaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        return projectile;
    }

    Multi_Projectile CallThrow(Multi_Projectile projectile, Transform target)
    {
        Multi_Enemy targetMonster = target == null ? null : target.GetComponent<Multi_Enemy>();
        Throw(projectile, new ThorwPathCalculator().CalculateThorwPath_To_Monster(targetMonster, projectileData.Attacker));
        return projectile;
    }

    public void Throw(Multi_Projectile projectile, Vector3 shotPath) => photonView.RPC(nameof(Multi_Throw), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, shotPath);
    Multi_Projectile FindProjectileWithId(int id) => Managers.Multi.GetPhotonViewTransfrom(id).GetComponent<Multi_Projectile>();
    [PunRPC]
    void Multi_Throw(int projectileId, Vector3 shotPath) => new ProjectileThrower().Thorw(FindProjectileWithId(projectileId), projectileData.SpawnPos, shotPath);


    // 최적화는 되지만 가끔씩 null 크래쉬나는 코드
    //Multi_Projectile CallThrow(Multi_Projectile projectile, Transform target)
    //{
    //    photonView.RPC(nameof(ThrowFlatY), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
    //    return projectile;
    //}
    //[PunRPC]
    //void ThrowFlatY(int projectileId, int targetId)
    //    => new ProjectileThrower().ThrowFlatY(FindProjectileWithId(projectileId), Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(targetId), projectileData.SpawnPos, projectileData.Attacker);
}

public class ProjectileThrower
{
    public void ThrowFlatY(Multi_Projectile projectile, Multi_Enemy target, Vector3 startPos, Transform attacker)
    {
        Vector3 path = new ThorwPathCalculator().CalculateThorwPath_To_Monster(target, attacker);
        path = new Vector3(path.x, 0, path.z);
        Thorw(projectile, startPos, path);
    }

    public void Thorw(Multi_Projectile projectile, Vector3 startPos, Vector3 shotPath)
    {
        projectile.transform.position = startPos;
        projectile.gameObject.SetActive(true);
        projectile.Throw(shotPath);
    }
}

public class ThorwPathCalculator
{
    public Vector3 CalculateThorwPath_To_Monster(Multi_Enemy target, Transform attacker)
    {
        if (target == null) return attacker.forward.normalized;
        else if (target.enemyType == EnemyType.Tower) return (target.transform.position - attacker.position).normalized;
        else return new ThorwPathCalculator().CalculatePath_To_MoveTarget(attacker.position, target.transform.position, target.GetComponent<Multi_NormalEnemy>().Speed, target.dir);
    }

    readonly float WEIGHT_RATE = 2f; // 움직이는 타겟 보간 가중치
    Vector3 CalculatePath_To_MoveTarget(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir)
    {
        Vector3 dir = targetPos - shoterPos;
        float enemyWeightDir = Mathf.Lerp(0, WEIGHT_RATE, Vector3.Distance(targetPos, shoterPos) * 2 / 100);
        dir += moveDir.normalized * (0.5f * speed) * enemyWeightDir;
        return dir.normalized;
    }
}
