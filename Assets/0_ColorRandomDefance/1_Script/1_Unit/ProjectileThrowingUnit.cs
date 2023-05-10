using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileThrowingUnit : MonoBehaviourPun
{
    ProjectileData projectileData;
    public void SetInfo(string weaponPath, Transform weaponThrowPoint) => projectileData = new ProjectileData(weaponPath, transform, weaponThrowPoint);

    public Multi_Projectile Throw(Transform target, System.Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(projectileData.WeaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        photonView.RPC(nameof(Throw), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
        return projectile;
    }

    [PunRPC]
    void Throw(int projectileId, int targetId)
    {
        var projectile = Managers.Multi.GetPhotonViewTransfrom(projectileId).GetComponent<Multi_Projectile>();
        projectile.transform.position = projectileData.SpawnPos;
        projectile.gameObject.SetActive(true);
        projectile.Throw(Get_ShootPath(projectileData.Attacker, Managers.Multi.GetPhotonViewTransfrom(targetId).GetComponent<Multi_Enemy>()));
    }

    Vector3 Get_ShootPath(Transform attacker, Multi_Enemy target)
    {
        if(target == null) return attacker.forward.normalized;

        if (target.enemyType == EnemyType.Tower) 
            return new ShotPathCalculator().Calculate_StaticTargetShotPath(attacker.position, target.transform.position);
        else
            return new ShotPathCalculator().Calculate_MovingTargetShotPath(attacker.position, target.transform.position, target.Speed, target.dir);
    }
}

public class ShotPathCalculator
{
    public Vector3 Calculate_StaticTargetShotPath(Vector3 shoterPos, Vector3 targetPos) => (targetPos - shoterPos).normalized;
    
    // target이 움직이고 있다면 가중치를 계산함
    readonly float WEIGHT_RATE = 2f;
    public Vector3 Calculate_MovingTargetShotPath(Vector3 shoterPos, Vector3 targetPos, float speed, Vector3 moveDir) 
    {
        Vector3 dir = targetPos - shoterPos;
        float enemyWeightDir = Mathf.Lerp(0, WEIGHT_RATE, Vector3.Distance(targetPos, shoterPos) * 2 / 100);
        dir += moveDir.normalized * (0.5f * speed) * enemyWeightDir;
        return dir.normalized;
    }
}
