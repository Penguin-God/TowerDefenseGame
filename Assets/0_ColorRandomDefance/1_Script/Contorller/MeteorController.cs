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
    {
        if(target == null)
        {
            print("메테오 target이 널임");
            return;
        }
        photonView.RPC(nameof(RPC_ShotMeteor), RpcTarget.All, target.GetComponent<PhotonView>().ViewID, hitDamage, stunTime, spawnPos);
    }

    public void ShotMeteor(Multi_Enemy target, int hitDamage, float stunTime, Vector3 spawnPos)
    {
        if (target == null)
        {
            print("메테오 target이 널임");
            return;
        }

        StartCoroutine(Co_ShotMeteor(target, HitAction, spawnPos));

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
        StartCoroutine(Co_ShotMeteor(target, hitAction, spawnPos));
    }

    IEnumerator Co_ShotMeteor(Multi_Enemy target, Action<Multi_Enemy> hitAction, Vector3 spawnPos)
    {
        var meteor = Managers.Resources.Instantiate(MeteorPath, spawnPos).GetComponent<Multi_Meteor>();
        Vector3 tempPos = target.transform.position;
        yield return new WaitForSeconds(1f);
        meteor.Shot(CalculateShotPoint(spawnPos, target, tempPos), hitAction);
    }

    Vector3 CalculateShotPoint(Vector3 meteorPos, Multi_Enemy enemy, Vector3 tempPos)
    {
        if (enemy == null || enemy.enemyType == EnemyType.Tower) return tempPos;
        else return (enemy.transform.position + enemy.dir.normalized * (enemy as Multi_NormalEnemy).Speed - meteorPos).normalized;
    }
}
