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

    public Multi_Projectile FlatThrow(Transform target, Action<Multi_Enemy> onHit) 
        => ThrowWithCalculator(nameof(CallFlat_Y_Throw), CreateProjectile(projectileData.WeaponPath, onHit), target);
    public Multi_Projectile FlatThrow(string weaponPath, Transform target, Action<Multi_Enemy> onHit)
        => ThrowWithCalculator(nameof(CallFlat_Y_Throw), CreateProjectile(weaponPath, onHit), target);

    Multi_Projectile ThrowWithCalculator(string rpcName, Multi_Projectile projectile, Transform target)
    {
        print(target == null);
        photonView.RPC(rpcName, RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
        return projectile;
    }

    public Multi_Projectile CreateProjectile(string weaponPath, Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(weaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        return projectile;
    }

    public void Throw(Multi_Projectile projectile, Vector3 shotPath)  => photonView.RPC(nameof(Multi_Throw), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, shotPath);
    [PunRPC] 
    void CallFlat_Y_Throw(int projectileId, int targetId)
    {
        Vector3 path = CalculateThorwPath(Managers.Multi.GetPhotonViewComponent<Multi_NormalEnemy>(targetId));
        path = new Vector3(path.x, 0, path.z);
        Multi_Throw(projectileId, path);
    }

    Vector3 CalculateThorwPath(Multi_NormalEnemy target)
    {
        if (target == null) return projectileData.Attacker.forward.normalized;
        else if (target.enemyType == EnemyType.Tower) return (target.transform.position - projectileData.Attacker.position).normalized;
        else return new ThorwPathCalculator().CalculatePath_To_MoveTarget(projectileData.Attacker.position, target.transform.position, target.Speed, target.dir);
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

public class ThorwPathCalculator
{
    readonly float WEIGHT_RATE = 2f; // 움직이는 타겟 보간 가중치
    public Vector3 CalculatePath_To_MoveTarget(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir)
    {
        Vector3 dir = targetPos - shoterPos;
        float enemyWeightDir = Mathf.Lerp(0, WEIGHT_RATE, Vector3.Distance(targetPos, shoterPos) * 2 / 100);
        dir += moveDir.normalized * (0.5f * speed) * enemyWeightDir;
        return dir.normalized;
    }
}
