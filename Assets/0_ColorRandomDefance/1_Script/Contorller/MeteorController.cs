using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeteorController : MonoBehaviourPun
{
    readonly string MeteorPath = new ResourcesPathBuilder().BuildEffectPath(SkillEffectType.Meteor);

    void HitAction(Multi_Enemy target, int hitDamage, float stunTime)
    {
        target.OnDamage(hitDamage, isSkill:true);
        target.OnStun_RPC(100, stunTime);
    }

    public void ShotMeteorToAll(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos) 
        => photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.All, target.GetComponent<PhotonView>().ViewID, hitDamage, stunTime, spawnPos);

    public void ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        ShotMeteor(target, HitAction, spawnPos);

        void HitAction(Multi_Enemy target)
        {
            target.OnDamage(hitDamage, isSkill: true);
            target.OnStun_RPC(100, stunTime);
        }
    }

    [PunRPC]
    void RPC_ShotMeteor(int viewId, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        var target = Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(viewId);
        Action<Multi_Enemy> hitAction = (_) => HitAction(_, hitDamage, stunTime);
        ShotMeteor(target, hitAction, spawnPos);
    }

    void ShotMeteor(Multi_Enemy target, Action<Multi_Enemy> hitAction, Vector3 spawnPos)
        => Managers.Resources.Instantiate(MeteorPath, spawnPos).GetComponent<Multi_Meteor>().ShotMeteor(target, hitAction);
}
