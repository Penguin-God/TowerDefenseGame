using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeteorController : MonoBehaviourPun
{
    readonly string MeteorPath = $"MageSkills/Meteor 1";

    void HitAction(Multi_Enemy target, int hitDamage, float stunTime)
    {
        target.OnDamage(hitDamage, isSkill:true);
        target.OnStun_RPC(100, stunTime);
    }

    public void ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        if(target == null)
        {
            print("메테오 target이 널임");
            return;
        }
        photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.MasterClient, target.GetComponent<PhotonView>().ViewID, hitDamage, stunTime, spawnPos);
    }

    [PunRPC]
    void RPC_ShotMeteor(int viewId, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        var target = Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(viewId);
        Action<Multi_Enemy> hitAction = (_) => HitAction(_, hitDamage, stunTime);
        StartCoroutine(Co_ShotMeteor(target, hitAction, spawnPos));
    }

    IEnumerator Co_ShotMeteor(Multi_Enemy target, Action<Multi_Enemy> hitAction, Vector3 spawnPos)
    {
        var meteor = WeaponSpawner.Spawn(MeteorPath, spawnPos).GetComponent<Multi_Meteor>();
        Vector3 tempPos = target.transform.position;
        yield return new WaitForSeconds(1f);

        if (target.IsDead)
            meteor.Shot(null, tempPos, hitAction);
        else
            meteor.Shot(target, target.transform.position, hitAction);
    }
}
