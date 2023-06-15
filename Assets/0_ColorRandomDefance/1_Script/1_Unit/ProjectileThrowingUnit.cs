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

    public Multi_Projectile Throw(Transform target, Action<Multi_Enemy> onHit) 
        => ThrowWithCalculator(nameof(CallThrow), CreateProjectile(projectileData.WeaponPath, onHit), target);
    public Multi_Projectile FlatThrow(Transform target, Action<Multi_Enemy> onHit) 
        => ThrowWithCalculator(nameof(CallFlatThrow), CreateProjectile(projectileData.WeaponPath, onHit), target);
    public Multi_Projectile FlatThrow(string weaponPath, Transform target, Action<Multi_Enemy> onHit)
        => ThrowWithCalculator(nameof(CallFlatThrow), CreateProjectile(weaponPath, onHit), target);

    Multi_Projectile ThrowWithCalculator(string rpcName, Multi_Projectile projectile, Transform target)
    {
        photonView.RPC(rpcName, RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
        return projectile;
    }

    Multi_Projectile CreateProjectile(string weaponPath, Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(weaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        return projectile;
    }

    public Multi_Projectile Throw(Vector3 shotPath, Action<Multi_Enemy> onHit)
    {
        var projectile = CreateProjectile(projectileData.WeaponPath, onHit);
        photonView.RPC(nameof(Multi_Throw), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, shotPath);
        return projectile;
    }

    [PunRPC] void CallThrow(int projectileId, int targetId) => ThrowWithCalculator(projectileId, targetId, new ShotPathCalculator());
    [PunRPC] void CallFlatThrow(int projectileId, int targetId) => ThrowWithCalculator(projectileId, targetId, new ZeroY_ShotPathCalculator());

    void ThrowWithCalculator(int projectileId, int targetId, ShotPathCalculator pathCalculator)
    {
        Vector3 path;
        Multi_Enemy target = Managers.Multi.GetPhotonViewTransfrom(targetId).GetComponent<Multi_Enemy>();
        if (target == null) path = projectileData.Attacker.forward.normalized;
        else if(target.enemyType == EnemyType.Tower)
            path = pathCalculator.CalculatePath_To_StaticTarget(projectileData.Attacker.position, target.transform.position);
        else
            path = pathCalculator.CalculatePath_To_MoveTarget(projectileData.Attacker.position, target.transform.position, target.Speed, target.dir);
        Multi_Throw(projectileId, path);
    }

    [PunRPC]
    void Multi_Throw(int projectileId, Vector3 shotPath)
    {
        var projectile = Managers.Multi.GetPhotonViewTransfrom(projectileId).GetComponent<Multi_Projectile>();
        projectile.transform.position = projectileData.SpawnPos;
        projectile.gameObject.SetActive(true);
        projectile.Throw(shotPath);
    }
}

public class ShotPathCalculator
{
    public virtual Vector3 CalculatePath_To_StaticTarget(Vector3 shoterPos, Vector3 targetPos) => (targetPos - shoterPos).normalized;
    
    // target이 움직이고 있다면 가중치를 계산함
    readonly float WEIGHT_RATE = 2f;
    public virtual Vector3 CalculatePath_To_MoveTarget(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir) 
    {
        Vector3 dir = targetPos - shoterPos;
        float enemyWeightDir = Mathf.Lerp(0, WEIGHT_RATE, Vector3.Distance(targetPos, shoterPos) * 2 / 100);
        dir += moveDir.normalized * (0.5f * speed) * enemyWeightDir;
        return dir.normalized;
    }
}

public class ZeroY_ShotPathCalculator : ShotPathCalculator
{
    public override Vector3 CalculatePath_To_StaticTarget(Vector3 shoterPos, Vector3 targetPos)
        => Y_To_Flat(base.CalculatePath_To_StaticTarget(shoterPos, targetPos));

    public override Vector3 CalculatePath_To_MoveTarget(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir)
        => Y_To_Flat(base.CalculatePath_To_MoveTarget(shoterPos, targetPos, speed, moveDir));

    Vector3 Y_To_Flat(Vector3 dir) => new Vector3(dir.x, 0, dir.z);
}
