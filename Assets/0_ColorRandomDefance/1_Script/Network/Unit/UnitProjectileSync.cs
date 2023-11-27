using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class UnitProjectileSync : MonoBehaviourPun
{
    UnitProjectile _unitProjectile;
    Action<Multi_Enemy> _attackAct;
    Vector3 _rotateVector;
    public void RegisterSyncProjectile(UnitProjectile unitProjectile, Action<Multi_Enemy> attackAct, Vector3 rotateVector)
    {
        _unitProjectile = unitProjectile;
        _attackAct = attackAct;
        _rotateVector = rotateVector;
    }

    public void SyncProjectileShot(Vector3 dir) => photonView.RPC(nameof(ProjectileShot), RpcTarget.AllBuffered, dir);

    [PunRPC] 
    void ProjectileShot(Vector3 dir)
    {
        _unitProjectile.AttackShot(dir, _attackAct);
        _unitProjectile.transform.Rotate(_rotateVector);
    }
}
