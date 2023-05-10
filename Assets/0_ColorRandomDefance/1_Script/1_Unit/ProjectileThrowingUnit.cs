using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class ProjectileThrowingUnit : MonoBehaviourPun
{
    ProjectileData projectileData;
    public void SetInfo(string weaponPath, Transform weaponThrowPoint) => projectileData = new ProjectileData(weaponPath, transform, weaponThrowPoint);

    public Multi_Projectile Throw(Transform target, Action<Multi_Enemy> onHit) => Throw(nameof(Throw), target, onHit);

    public Multi_Projectile FlatThrow(Transform target, Action<Multi_Enemy> onHit) => Throw(nameof(FlatThrow), target, onHit);

    public Multi_Projectile Throw(string rpcMethodName, Transform target, Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(projectileData.WeaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        photonView.RPC(rpcMethodName, RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
        return projectile;
    }

    [PunRPC]
    void Throw(int projectileId, int targetId) => Throw(projectileId, targetId, new ShotPathCalculator());

    [PunRPC]
    void FlatThrow(int projectileId, int targetId) => Throw(projectileId, targetId, new ZeroY_ShotPathCalculator());

    void Throw(int projectileId, int targetId, ShotPathCalculator pathCalculator)
    {
        var projectile = Managers.Multi.GetPhotonViewTransfrom(projectileId).GetComponent<Multi_Projectile>();
        projectile.transform.position = projectileData.SpawnPos;
        projectile.gameObject.SetActive(true);
        projectile.Throw(Get_ShootPath(projectileData.Attacker, Managers.Multi.GetPhotonViewTransfrom(targetId).GetComponent<Multi_Enemy>(), pathCalculator));
    }

    Vector3 Get_ShootPath(Transform attacker, Multi_Enemy target, ShotPathCalculator pathCalculator)
    {
        if(target == null) return attacker.forward.normalized;

        if (target.enemyType == EnemyType.Tower)
            return pathCalculator.CalculatePath_To_StaticTarget(attacker.position, target.transform.position);
        else
            return pathCalculator.CalculatePath_To_MoveTarget(attacker.position, target.transform.position, target.Speed, target.dir);
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
