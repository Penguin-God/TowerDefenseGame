using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MeteorController : MonoBehaviourPun
{
    readonly string MeteorPath = new ResourcesPathBuilder().BuildEffectPath(SkillEffectType.Meteor);
    WorldAudioPlayer _audioPlayer;

    public void DepencyInject(WorldAudioPlayer audioPlayer)
    {
        _audioPlayer = audioPlayer;
    }

    void HitAction(Multi_Enemy target, int hitDamage, float stunTime)
    {
        target.OnDamage(hitDamage, isSkill:true);
        target.OnStun_RPC(100, stunTime);
    }

    public void ShotMeteorToAll(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos, byte id)
        => photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.All, target.GetComponent<PhotonView>().ViewID, hitDamage, stunTime, spawnPos, id);

    public void ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos, ObjectSpot spot)
    {
        ShotMeteor(target, HitAction, spawnPos, spot);

        void HitAction(Multi_Enemy target)
        {
            target.OnDamage(hitDamage, isSkill: true);
            target.OnStun_RPC(100, stunTime);
        }
    }

    [PunRPC]
    void RPC_ShotMeteor(int viewId, int hitDamage, float stunTime, Vector3 spawnPos, byte worldId)
    {
        var target = Managers.Multi.GetPhotonViewComponent<Multi_Enemy>(viewId);
        Action<Multi_Enemy> hitAction = (_) => HitAction(_, hitDamage, stunTime);
        ShotMeteor(target, hitAction, spawnPos, new ObjectSpot(worldId, true));
    }

    void ShotMeteor(Multi_Enemy target, Action<Multi_Enemy> hitAction, Vector3 spawnPos, ObjectSpot spot)
    {
        var meteor = Managers.Resources.Instantiate(MeteorPath, spawnPos).GetComponent<Multi_Meteor>();
        meteor.DependencyInject(hitAction, _audioPlayer, spot);
        meteor.ShotMeteor(target);
    }
}
