using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileThrowingUnit : MonoBehaviourPun
{
    ProjectileData projectileData;
    public void SetInfo(string weaponPath, Transform weaponThrowPoint)
        => projectileData = new ProjectileData(weaponPath, transform, weaponThrowPoint);

    public void Throw(Transform target, System.Action<Multi_Enemy> onHit)
    {
        var projectile = Managers.Multi.Instantiater.PhotonInstantiateInactive(projectileData.WeaponPath, PlayerIdManager.InVaildId).GetComponent<Multi_Projectile>();
        projectile.SetHitAction(onHit);
        photonView.RPC(nameof(Throw), RpcTarget.All, projectile.GetComponent<PhotonView>().ViewID, target.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    void Throw(int projectileId, int targetId)
    {
        var projectile = Managers.Multi.GetPhotonViewTransfrom(projectileId).GetComponent<Multi_Projectile>();
        projectile.transform.position = projectileData.SpawnPos;
        projectile.gameObject.SetActive(true);
        projectile.Throw(Get_ShootDirection(projectileData.Attacker, Managers.Multi.GetPhotonViewTransfrom(targetId)));
    }

    Vector3 Get_ShootDirection(Transform attacker, Transform _target, float weightRate = 2f)
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
